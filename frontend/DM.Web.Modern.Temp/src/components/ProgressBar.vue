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
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.progress
  position: relative
  overflow: hidden
  margin: Variables.$small 0
  padding: Variables.$small
  border-radius: Variables.$border-radius
  +Themes.theme(background-color, Themes.$progress-background)

.progress-scale
  position: absolute
  top: 0
  left: 0
  bottom: 0
  transition: width Variables.$animation-time
  +Themes.theme(background-color, Themes.$progress-background-done)

.progress-text
  position: relative
  +Themes.theme(color, Themes.$text)
</style>
