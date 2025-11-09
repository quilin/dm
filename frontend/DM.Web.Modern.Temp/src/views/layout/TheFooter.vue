<template>
  <div class="footer">
    <secondary-text class="copyright">
      © dm.am, 2007 &ndash; {{ currentYear }}
    </secondary-text>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";

const currentYear = ref<number>(new Date().getFullYear());
</script>

<style scoped lang="sass">
@use "@/assets/styles/Layout"
@use "@/assets/styles/Themes"

.footer
  display: flex
  justify-content: center
  position: relative
  box-sizing: border-box
  height: Layout.$footer-height
  margin-top: -(Layout.$footer-height)
  line-height: Layout.$footer-height

  &:before
    content: ''
    position: absolute
    top: 0
    left: 0
    right: 0
    bottom: 0
    background: url('@/assets/images/footer_bg.gif') left top repeat-x
    +Themes.theme(filter, Themes.color-pair(none, invert(87%)))

.copyright
  align-self: center
  position: relative
  text-align: center
</style>
