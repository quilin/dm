<template>
  <div class="progress">
    <div class="progress-scale" :style="{ width: `${progress}%` }" />
    <div class="progress-text"><slot /></div>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";

const props = defineProps<{ goal: number; current: number }>();
const progress = computed(() => (props.current / props.goal) * 100);
</script>

<style scoped lang="sass">
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
  +theme(background-color, $progress-background-done)
  transition: width $animation-time

.progress-text
  position: relative
  +theme(color, $text)
</style>
