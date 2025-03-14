<script setup lang="ts">
import { ref, computed, onMounted, watch, toRaw } from 'vue'
import { Plus, Trash2, Lock, Unlock, FileText, Pencil, Check, X } from 'lucide-vue-next'
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Switch } from '@/components/ui/switch'
import { Label } from '@/components/ui/label'
import { ScrollArea } from '@/components/ui/scroll-area'
import { Badge } from '@/components/ui/badge'
import type { Chapter, Text } from '@/types/types'
import * as chapterService from '@/services/chapter.service'
import * as textService from '@/services/text.service'
import Textarea from '@/components/ui/textarea/Textarea.vue'

// --- State Management ---
const chapters = ref<Chapter[]>([])
const texts = ref<Text[]>([])
const selectedChapterId = ref<string | null>(null)

// Forms & Editing State
const newChapterName = ref('')
const isPrivate = ref(false)
const newTextContent = ref('')

const editingChapterId = ref<string | null>(null)
const editChapterName = ref('')

const editingTextId = ref<string | null>(null)
const editTextContent = ref('')

// --- Fetch Functions ---
const fetchChapters = async () => {
  try {
    chapters.value = await chapterService.all()
  } catch (error) {
    console.error('Failed to fetch chapters:', error)
  }
}

const fetchTexts = async (chapterId: string) => {
  try {
    texts.value = await textService.getByTextId(chapterId)
  } catch (error) {
    console.error('Failed to fetch texts:', error)
  }
}

// --- Lifecycle ---
onMounted(async () => {
  await fetchChapters()
})

watch(selectedChapterId, async (newId) => {
  if (newId) {
    await fetchTexts(newId)
  } else {
    texts.value = []
  }
})

// --- Computed ---
const selectedChapter = computed(() => chapters.value.find((c) => c.id === selectedChapterId.value))
const filteredTexts = computed(() =>
  texts.value.filter((t) => t.chapterId === selectedChapterId.value),
)

// Upload file
const fileInput = ref<HTMLInputElement | null>(null)

function openFilePicker() {
  fileInput.value?.click()
}

function handleFileUpload(event: Event) {
  const input = event.target as HTMLInputElement
  const file = input.files?.[0]

  if (!file) return

  const reader = new FileReader()

  reader.onload = (e) => {
    newTextContent.value = e.target?.result as string
  }

  reader.readAsText(file)
}

// --- Actions (CRUD) ---

// Chapter Logic
const addChapter = async () => {
  if (!newChapterName.value.trim()) return
  try {
    await chapterService.create({ name: newChapterName.value, isPrivate: isPrivate.value })
    await fetchChapters()
    newChapterName.value = ''
  } catch (error) {
    console.error('Failed to create chapter:', error)
  }
}

const startEditChapter = (chapter: Chapter) => {
  editingChapterId.value = chapter.id
  editChapterName.value = chapter.name
}

const saveChapterUpdate = async () => {
  const chapter = chapters.value.find(c => c.id === editingChapterId.value)
  if (chapter && editChapterName.value.trim()) {
    try {
      await chapterService.update(chapter.id, { name: editChapterName.value, isPrivate: chapter.isPrivate })
      await fetchChapters()
    } catch (error) {
      console.error('Failed to update chapter:', error)
    }
  }
  editingChapterId.value = null
}

const deleteChapter = async (id: string) => {
  try {
    await chapterService.del(id)
    await fetchChapters()
    texts.value = texts.value.filter((t) => t.chapterId !== id)
    if (selectedChapterId.value === id) selectedChapterId.value = null
  } catch (error) {
    console.error('Failed to delete chapter:', error)
  }
}

// Text Logic
const addText = async () => {
  if (!newTextContent.value.trim() || !selectedChapterId.value) return
  try {
    await textService.create(selectedChapterId.value, { content: newTextContent.value })
    await fetchTexts(selectedChapterId.value)
    newTextContent.value = ''
  } catch (error) {
    console.error('Failed to create text:', error)
  }
}

const startEditText = (text: Text) => {
  editingTextId.value = text.id
  editTextContent.value = text.content
}

const saveTextUpdate = async () => {
  const text = texts.value.find(t => t.id === editingTextId.value)
  if (text && editTextContent.value.trim()) {
    try {
      await textService.update(text.id, { content: editTextContent.value })
      await fetchTexts(selectedChapterId.value!)
    } catch (error) {
      console.error('Failed to update text:', error)
    }
  }
  editingTextId.value = null
}

const deleteText = async (id: string) => {
  try {
    await textService.del(id)
    await fetchTexts(selectedChapterId.value!)
  } catch (error) {
    console.error('Failed to delete text:', error)
  }
}

// redirect to game page test
function enterGamePage(){
  const rawSelectedChapter = toRaw(selectedChapter.value);
  const rawTexts = texts.value.map(item => toRaw(item));

  localStorage.setItem('mrtyper_chapter_text_data', JSON.stringify({
    'chapter': rawSelectedChapter,
    'text': JSON.stringify(rawTexts)
  }))
}
</script>

<template>
  <div class="w-screen mx-auto grid grid-cols-12 gap-8 p-8 h-screen">
    <Card class="col-span-4 flex flex-col justify-start shadow-sm border-muted-foreground/10">
      <CardHeader class="pb-0">
        <CardTitle class="text-xl font-bold tracking-tight">Chapters</CardTitle>
        <CardDescription>Organize your thoughts into sections.</CardDescription>

        <div class="space-y-3 mt-4 py-5">
          <Input
            v-model="newChapterName"
            placeholder="New chapter title..."
            class="bg-muted/30"
            @keyup.enter="addChapter"
          />
          <div class="flex items-center justify-between px-1 py-2">
            <div class="flex items-center space-x-2">
              <Switch id="private-mode" v-model:checked="isPrivate" />
              <Label for="private-mode" class="text-xs font-medium cursor-pointer pl-3"
                >Private</Label
              >
            </div>
            <Button @click="addChapter" size="sm" :disabled="!newChapterName">
              <Plus class="w-4 h-4 mr-1.5" /> Create
            </Button>
          </div>
        </div>
      </CardHeader>

      <CardContent class="grow overflow-hidden w-full">
        <ScrollArea class="h-full w-full">
          <div
            v-if="chapters.length === 0"
            class="text-center text-muted-foreground text-sm border-2 border-dashed rounded-xl p-4"
          >
            No chapters yet.
          </div>

          <div
            v-for="chapter in chapters"
            :key="chapter.id"
            @click="selectedChapterId = chapter.id"
            style="margin-bottom: 20px"
            :class="[
              'w-full group p-3 mb-2 rounded-xl cursor-pointer border transition-all flex justify-between items-center',
              selectedChapterId === chapter.id
                ? 'bg-primary/70 text-primary-foreground shadow-md border-primary'
                : 'bg-accent hover:bg-muted border-transparent hover:border-muted-foreground/90',
            ]"
          >
            <div class="flex items-center gap-3 truncate">
              <component
                :is="chapter.isPrivate ? Lock : Unlock"
                class="w-4 h-4 shrink-0 opacity-70"
              />
              <span class="font-medium truncate">{{ chapter.name }}</span>
            </div>

            <Button
              variant="ghost"
              size="icon"
              class="h-8 w-8 text-destructive hover:bg-destructive/20"
              :class="
                selectedChapterId === chapter.id ? 'text-primary-foreground hover:bg-white/20' : ''
              "
              @click.stop="deleteChapter(chapter.id)"
            >
              <Trash2 class="w-4 h-4" />
            </Button>
          </div>
        </ScrollArea>
      </CardContent>
    </Card>

    <Card class="col-span-8 flex flex-col shadow-sm border-muted-foreground/10 overflow-hidden">
      <template v-if="selectedChapter">
        <CardHeader class="border-b bg-muted/10">
          <div class="flex justify-between items-center">
            <div class="flex items-center gap-4 flex-grow">
              <div
                v-if="editingChapterId === selectedChapter.id"
                class="flex items-center gap-2 flex-grow"
              >
                <Input v-model="editChapterName" class="h-9" @keyup.enter="saveChapterUpdate" />
                <Button size="icon" variant="ghost" class="h-8 w-8" @click="saveChapterUpdate"
                  ><Check class="w-4 h-4 text-green-500"
                /></Button>
              </div>
              <div v-else class="flex items-center justify-between w-full gap-2 group">
                <div class="flex items-center gap-2">
                  <CardTitle class="text-xl">{{ selectedChapter.name }}</CardTitle>
                  <Button
                    variant="ghost"
                    size="icon"
                    class="h-6 w-6"
                    @click="startEditChapter(selectedChapter)"
                  >
                    <Pencil class="w-3.5 h-3.5" />
                  </Button>
                </div>
                <Button class="w-40 h-10 text-xl" @click="enterGamePage">
                  Play
                </Button>
              </div>
            </div>
            <Badge v-if="selectedChapter.isPrivate" variant="outline" class="bg-background"
              >Private Chapter</Badge
            >
          </div>
        </CardHeader>

        <CardContent class="flex-grow flex flex-col gap-6 p-6 overflow-hidden">
          <div
            class="flex flex-col p-3 gap-2 bg-muted/20 rounded-xl border border-muted-foreground/10 shadow-inner"
          >
            <Textarea
              v-model="newTextContent"
              placeholder="What's on your mind?..."
              class="border-none bg-transparent focus-visible:ring-0 min-h-25 resize-none text-sm leading-relaxed"
              @keydown.enter.ctrl.prevent="addText"
              @keydown.enter.meta.prevent="addText"
            />
            <div class="flex justify-end gap-2">
              <Button @click="openFilePicker" class="rounded-lg shadow-sm" variant="secondary">Upload file</Button>
              <Button @click="addText" class="rounded-lg shadow-sm">Add Entry</Button>
              <input type="file" @change="handleFileUpload" class="hidden" ref="fileInput">
            </div>
          </div>

          <ScrollArea class="flex-grow pr-4 h-10">
            <div class="space-y-4">
              <div
                v-for="text in filteredTexts"
                :key="text.id"
                class="group relative p-4 border rounded-xl bg-card hover:bg-muted/20 transition-all hover:shadow-sm"
                style="margin-bottom: 20px;"
              >
                <div v-if="editingTextId === text.id" class="flex flex-col gap-2">
                  <Textarea
                    v-model="editTextContent"
                    class="w-full bg-secondary border rounded-md p-2 text-sm outline-none ring-1 ring-primary"
                    @keydown.enter.ctrl.prevent="addText"
                    @keydown.enter.meta.prevent="addText"
                  />
                  <div class="flex justify-end gap-2">
                    <Button size="sm" variant="ghost" @click="editingTextId = null"
                      ><X class="w-3 h-3 mr-1" /> Cancel</Button
                    >
                    <Button size="sm" @click="saveTextUpdate"
                      ><Check class="w-3 h-3 mr-1" /> Save</Button
                    >
                  </div>
                </div>

                <div v-else class="cursor-pointer" @click="startEditText(text)">
                  <p class="text-sm leading-relaxed text-foreground/80">{{ text.content }}</p>
                  <div
                    class="absolute bottom-2 right-2 flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity bg-background p-0.5 rounded outline"
                  >
                    <Button
                      variant="ghost"
                      size="icon"
                      class="h-8 w-8"
                      @click.stop="startEditText(text)"
                    >
                      <Pencil class="w-3.5 h-3.5" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="icon"
                      class="h-8 w-8 text-destructive hover:bg-destructive/10"
                      @click.stop="deleteText(text.id)"
                    >
                      <Trash2 class="w-3.5 h-3.5" />
                    </Button>
                  </div>
                </div>
              </div>

              <div
                v-if="filteredTexts.length === 0"
                class="text-center py-20 text-muted-foreground/60"
              >
                <p class="italic">No text entries in this chapter yet.</p>
              </div>
            </div>
          </ScrollArea>
        </CardContent>
      </template>

      <div
        v-else
        class="flex flex-col items-center justify-center h-full text-muted-foreground/40 bg-muted/5"
      >
        <div class="p-6 rounded-full bg-muted/20 mb-4">
          <FileText class="w-12 h-12" />
        </div>
        <p class="text-lg font-medium">Select a chapter to begin</p>
        <p class="text-sm">Pick a chapter from the left to view or edit content.</p>
      </div>
    </Card>
  </div>
</template>
