<script setup lang="ts">
import { GalleryVerticalEnd } from "lucide-vue-next"

import SignupForm from "@/components/new-york-v4/blocks/signup-02/components/SignupForm.vue"
import TypeAnimation from "@/components/TypeAnimation.vue";
import ScrollArea from "@/components/ui/scroll-area/ScrollArea.vue";
</script>

<template>
  <div class="grid min-h-svh lg:grid-cols-2">
    <ScrollArea class="h-screen w-full rounded-md border p-4">
      <div class="flex flex-col gap-4 p-6 md:p-10">
        <div class="flex justify-center gap-2 md:justify-start">
          <a href="#" class="flex items-center gap-2 font-medium">
            <div class="bg-primary text-primary-foreground flex size-6 items-center justify-center rounded-md">
              <GalleryVerticalEnd class="size-4" />
            </div>
            MrTyper.
          </a>
        </div>
        <div class="flex flex-1 items-center justify-center">
          <div class="w-full max-w-xs">
            <SignupForm />
          </div>
        </div>
      </div>
    </ScrollArea>
    <div class="bg-muted relative flex items-center justify-center w-full h-full">
      <TypeAnimation/>
    </div>
  </div>
</template>
