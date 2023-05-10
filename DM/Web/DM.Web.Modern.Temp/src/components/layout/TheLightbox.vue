<template>
  <vue-final-modal
    content-class="lightbox"
    overlay-transition="vfm-fade"
    content-transition="vfm-fade"
  >
    <slot />
  </vue-final-modal>
</template>

<script setup lang="ts">
import { VueFinalModal } from "vue-final-modal";
</script>

<style lang="sass">
.vfm
  display: flex
  justify-content: center
  align-items: center

.lightbox
  +theme(background-color, $background)
  padding: $medium
  width: $grid-step * 100
  & h2
    margin-top: 0
</style>
