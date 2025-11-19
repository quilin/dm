<script setup lang="ts">
import { FloatLabel } from "primevue";

defineProps<{ id: string; placeholder?: string; isFilled: boolean }>();
</script>

<template>
  <FloatLabel variant="on">
    <slot />
    <label v-if="placeholder" :for="id" :class="{ 'is-filled': isFilled }">{{
      placeholder
    }}</label>
  </FloatLabel>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

[data-pc-name="floatlabel"]
  display: inline-flex
  position: relative

  & label
    position: absolute
    left: Variables.$small
    top: 50%
    transform: translateY(-50%)
    pointer-events: none

    +Themes.theme(color, Themes.$secondary-text)
    +Themes.theme(background-color, Themes.$background)
    transition: all Variables.$animation-time !important

  &:has(input:focus) label,
  label.is-filled
    top: 0
    font-size: Variables.$secondary-font-size
</style>
