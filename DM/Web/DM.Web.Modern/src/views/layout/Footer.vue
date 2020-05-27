<template>
  <div class="footer">
    <div class="copyright">© dm.am, 2007 &ndash; {{ currentYear }}</div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class DmFooter extends Vue {
  get currentYear(): number {
    return new Date().getFullYear();
  }
}
</script>

<style scoped lang="stylus">
.footer
  display flex
  justify-content center
  position relative
  box-sizing border-box
  height $footerHeight
  margin-top -(@height)
  line-height $footerHeight

  &:before
    content ''
    position absolute
    left 0
    right 0
    top 0
    bottom 0
    background url('~@/assets/footer_bg.gif') left top repeat-x
    theme(filter, colorPair(none, invert(87%)))
    transition filter $animationTime

.copyright
  align-self center
  position relative
  text-align center
  secondary()
</style>
