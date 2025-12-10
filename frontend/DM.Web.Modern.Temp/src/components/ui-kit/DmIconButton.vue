<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";

defineProps<{
  icon: IconType;
  loading?: boolean;
  type?: "reset" | "submit" | "button";
}>();
</script>

<template>
  <button
    :type="type ?? 'button'"
    :class="['icon-button', loading && 'loading']"
  >
    <dm-icon :font="icon" />
  </button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"
@use "@/assets/styles/Layout"

.icon-button
  +Layout.square(Variables.$grid-step * 5)
  padding: Variables.$tiny
  border: none

  +Themes.theme(background-color, Themes.$background)
  +Themes.theme(color, Themes.$active-text)

  font-size: Variables.$font-size
  line-height: 1
  text-align: center

  border-radius: Variables.$medium
  cursor: pointer

  &:hover
    +Themes.theme(background-color, Themes.$panel-background-hover)
    +Themes.theme(color, Themes.$active-text-hover)

  &.loading
    background-position: center center
    background-image: url('@/assets/images/loader.gif')
    background-size: Variables.$medium
    background-repeat: no-repeat
    color: transparent !important
    cursor: progress
</style>
