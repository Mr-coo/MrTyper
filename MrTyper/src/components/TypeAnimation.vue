<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Typed from 'typed.js'

const el = ref()

onMounted(() => {
  new Typed(el.value, {
    strings: ['Compete with friends', 'See your WPM', 'Push your typing speed'],
    typeSpeed: 80,
    backSpeed: 40,
    loop: true,
  })
})
</script>

<template>
  <span ref="el" class="text-3xl"></span>
</template>
