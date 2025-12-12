<script setup lang="ts">
import type { User } from "@/api/models/community";

const props = defineProps<{ user: User }>();
</script>

<template>
  <router-link
    class="rating"
    :to="{ name: 'profile', params: { login: props.user.login } }"
  >
    <template v-if="user.rating.enabled">
      <span
        :class="{
          quality: true,
          positive: props.user.rating.quality > 0,
          negative: props.user.rating.quality < 0,
        }"
        >{{ props.user.rating.quality }}</span
      >/{{ props.user.rating.quantity }}
    </template>
    <template v-else>скрыт</template>
  </router-link>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Themes"

.quality
  font-weight: bold

.positive
  +Themes.theme(color, Themes.$positive-text)

.negative
  +Themes.theme(color, Themes.$negative-text)
</style>
