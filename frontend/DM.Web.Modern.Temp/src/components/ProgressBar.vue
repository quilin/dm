<script setup lang="ts">
import { computed } from "vue";

const props = defineProps<{ goal: number; current: number }>();
const progress = computed(() => (props.current / props.goal) * 100);
</script>

<template>
  <div class="progress">
    <div class="progress-scale" :style="{ width: `${progress}%` }" />
    <div class="progress-text"><slot /></div>
  </div>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Themes"

.progress
  position: relative
  overflow: hidden
  margin: $small 0
  padding: $minor
  border-radius: $border-radius
  +theme(background-color, $progress-background)

.progress-scale
  position: absolute
  top: 0
  left: 0
  bottom: 0
  transition: width $animation-time
  +theme(background-color, $progress-background-done)

.progress-text
  position: relative
  +theme(color, $text)
</style>
