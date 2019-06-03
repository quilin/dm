<template>
  <div id="app" :class="['main', `theme_${theme}`]">
    <div class="content">
      <dm-header />
      <div class="content-body">
        <router-view />
      </div>
    </div>
    <dm-footer />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import DmFooter from '@/views/layout/Footer.vue';
import DmHeader from '@/views/layout/Header.vue';

@Component({
  components: {
    DmHeader,
    DmFooter,
  },
})
export default class DmApp extends Vue {
  private theme : string = '';
}
</script>

<style lang="stylus">
@import '~@/styles/Fonts'
@import '~@/styles/Variables'
@import '~@/styles/Themes'

html, body
  height 100%
  margin 0

.main
  position relative
  height 100%
  min-height 100%
  min-width 1000px
  max-width 1400px
  overflow-y scroll

.content
  height 100%
  theme(color, $text)
  theme(background-color, $background)

</style>
