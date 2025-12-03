<script setup lang="ts">
import type { User } from "@/api/models/community";

defineProps<{ user: User }>();
</script>

<template>
  <router-link
    class="user-picture-container"
    :to="{ name: 'profile', params: { login: user.login } }"
  >
    <div
      class="user-picture"
      :style="{
        backgroundImage:
          user.mediumPictureUrl && `url(${user.mediumPictureUrl})`,
      }"
    ></div>
  </router-link>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.user-picture-container
  display: block
  height: Variables.$grid-step * 10
  flex-shrink: 0

.user-picture
  width: Variables.$grid-step * 10
  height: Variables.$grid-step * 10
  margin-right: Variables.$grid-step * 3
  background: url('@/assets/images/userpic.png') transparent left center no-repeat
  background-size: contain
  border-radius: Variables.$big
</style>
