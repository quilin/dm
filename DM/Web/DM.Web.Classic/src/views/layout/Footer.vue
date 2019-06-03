<template>
  <div class="footer">
    <div class="copyright">© DungeonMaster.ru, 2007 &ndash; {{ currentYear }}</div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class DmFooter extends Vue {
  get currentYear(): number {
    return new Date().getFullYear();
  }
}
</script>

<style scoped lang="stylus">
.footer
  height 40px
  background transparent url('~@/assets/footer_bg.gif') left bottom repeat-x
  margin-top -90px
  padding-top 50px
</style>
