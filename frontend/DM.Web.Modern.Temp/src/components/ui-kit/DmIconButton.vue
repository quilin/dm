<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";

defineProps<{
  icon: IconType;
  loading?: boolean;
  disabled?: boolean;
  type?: "reset" | "submit" | "button";
}>();
</script>

<template>
  <button
    :type="type ?? 'button'"
    :disabled="disabled"
    :class="[
      'icon_button',
      loading && 'icon_button-loading',
      !disabled && 'icon_button-active',
    ]"
  >
    <dm-icon :font="icon" /><slot />
  </button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"
@use "@/assets/styles/Layout"

.icon_button
  +Layout.square(Variables.$grid-step * 5)
  display: inline-block
  padding: Variables.$tiny
  width: initial
  min-width: Variables.$grid-step * 5
  border: none

  +Themes.theme(background-color, Themes.$background)
  +Themes.theme(color, Themes.$active-text)

  font-family: "PT Sans", sans-serif
  line-height: 1
  text-align: center

  border-radius: Variables.$medium
  cursor: default

  &.icon_button-active
    cursor: pointer
    &:hover
      +Themes.theme(background-color, Themes.$panel-background-hover)
      +Themes.theme(color, Themes.$active-text-hover)

  &.icon_button-loading
    background-position: center center
    background-image: url('@/assets/images/loader.gif')
    background-size: Variables.$medium
    background-repeat: no-repeat
    color: transparent !important
    cursor: progress
</style>
