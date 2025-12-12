<script setup lang="ts">
import { VueFinalModal } from "vue-final-modal";
defineProps<{ withForm?: boolean }>();
</script>

<template>
  <vue-final-modal
    overlay-transition="vfm-fade"
    content-transition="vfm-fade"
    :click-to-close="!withForm"
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

.vfm__content
  position: relative
  padding: 0 Variables.$medium Variables.$medium
  max-width: Variables.$grid-step * 100
  border-radius: Variables.$border-radius
  +Themes.theme(background-color, Themes.$background)
</style>
