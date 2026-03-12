<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Cookies from 'js-cookie'
import { toast } from 'vue-sonner'
import TypeAnimation from '@/components/TypeAnimation.vue'

const route = useRoute()
const router = useRouter()

onMounted(() => {
  const accessToken = route.query.accessToken
  const refreshToken = route.query.refreshToken
  const message = route.query.message

  if (accessToken && refreshToken) {
    Cookies.set('access_token', accessToken, { expires: 1, secure: true, sameSite: 'strict' })
    Cookies.set('refresh_token', refreshToken, { expires: 7, secure: true, sameSite: 'strict' })

    if (message) toast.info(message)

    router.push('/')
  } else {
    if (message) toast.error(message)
    router.push('/login')
  }
})
</script>

<template>
  <div>
    <TypeAnimation/>
  </div>
</template>
