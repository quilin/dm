<script setup lang="ts">
import { VueFinalModal } from "vue-final-modal";
</script>

<template>
  <vue-final-modal
    content-class="lightbox"
    overlay-transition="vfm-fade"
    content-transition="vfm-fade"
  >
    <slot />
  </vue-final-modal>
</template>

<style lang="sass">
.vfm
  display: flex
  justify-content: center
  align-items: center

.lightbox
  position: relative
  padding: $medium
  width: $grid-step * 100
  border-radius: $border-radius
  +theme(background-color, $background)

  & h2
    margin-top: 0

.lightbox-close
  position: absolute
  top: 0
  right: 0
  margin: $small

  font-size: $grid-step * 4
  cursor: pointer
  +theme(color, $text)

  &:hover
    +theme(color, $active-text)

h2
  margin-top: 0
</style>
