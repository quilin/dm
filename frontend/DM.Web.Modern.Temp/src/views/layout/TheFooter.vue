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
.footer
  display: flex
  justify-content: center
  position: relative
  box-sizing: border-box
  height: $footer-height
  margin-top: -$footer-height
  line-height: $footer-height

  &:before
    content: ''
    position: absolute
    top: 0
    left: 0
    right: 0
    bottom: 0
    background: url('@/assets/images/footer_bg.gif') left top repeat-x
    +theme(filter, color-pair(none, invert(87%)))
    transition: filter $animation-time

.copyright
  align-self: center
  position: relative
  text-align: center
</style>
