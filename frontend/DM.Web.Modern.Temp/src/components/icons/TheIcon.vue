<template>
  <span class="icon" v-html="charCode" />
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { IconType } from "@/components/icons/iconType";

const props = defineProps<{ font: IconType }>();

const charCode = computed(() => `&#x${props.font.toString(16)};`);
</script>
<style scoped lang="sass">
@use "@/assets/styles/Layout"

.icon
  +Layout.icon()
</style>
