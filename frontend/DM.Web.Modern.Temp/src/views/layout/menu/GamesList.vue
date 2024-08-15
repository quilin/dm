<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { IconType } from "@/components/icons/iconType";
import { GameStatus } from "@/api/models/gaming";

defineProps<{
  token: string;
  title: string;
  status: GameStatus;
  linkText: string;
}>();
</script>

<template>
  <menu-block :token="token">
    <template #title>{{ title }}</template>
    <slot />
    <router-link
      class="forward"
      :to="{ name: 'games', params: { status: status.toLowerCase() } }"
    >
      {{ linkText }}
      <the-icon :font="IconType.Forward" />
    </router-link>
  </menu-block>
</template>

<style scoped lang="sass">
.forward
  font-weight: bold
</style>
