<template>
  <menu-block :token="props.token">
    <template #title>{{ props.title }}</template>
    <slot />
    <router-link
      class="forward"
      :to="{ name: 'games', params: { status: props.status.toLowerCase() } }"
    >
      {{ props.linkText }}
      <the-icon :font="IconType.Forward" />
    </router-link>
  </menu-block>
</template>

<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { IconType } from "@/components/icons/iconType";
import { GameStatus } from "@/api/models/gaming";

const props = defineProps<{
  token: string;
  title: string;
  status: GameStatus;
  linkText: string;
}>();
</script>

<style scoped lang="sass">
.forward
  font-weight: bold
</style>
