<script setup lang="ts">
import { VueFinalModal } from "vue-final-modal";
</script>

<template>
  <vue-final-modal
    content-class="lightbox"
    overlay-transition="vfm-fade"
    content-transition="vfm-fade"
    ><slot
  /></vue-final-modal>
</template>

<style lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.vfm
  display: flex
  justify-content: center
  align-items: center

.lightbox
  position: relative
  padding: Variables.$medium
  width: Variables.$grid-step * 100
  border-radius: Variables.$border-radius
  +Themes.theme(background-color, Themes.$background)

  & h2
    margin-top: 0

.lightbox-close
  position: absolute
  top: 0
  right: 0
  margin: Variables.$small

  font-size: Variables.$grid-step * 4
  cursor: pointer
  +Themes.theme(color, Themes.$text)

  &:hover
    +Themes.theme(color, Themes.$active-text)

h2
  margin-top: 0
</style>
