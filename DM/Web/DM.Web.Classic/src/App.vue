<template>
  <div id="app" :class="`theme_${currentTheme}`">
    <div class="main">
      <div class="content-container">
        <div class="content-wrapper">
          <dm-header />
          <div class="content-body">
            <div class="content-menu">
              <router-view name="menu" />
            </div>
            <div class="content">
              <router-view name="page" />
            </div>
            <div class="content-sidebar">
              <router-view name="sidebar" />
              Hello, world!
            </div>
          </div>
        </div>
        <dm-footer />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { mapGetters } from 'vuex';
import DmFooter from '@/views/layout/Footer.vue';
import DmHeader from '@/views/layout/Header.vue';

@Component({
  components: {
    DmHeader,
    DmFooter,
  },
  computed: {
    ...mapGetters(['currentTheme']),
  },
})
export default class DmApp extends Vue {}
</script>

<style lang="stylus">
html, body, #app
  height 100%
  margin 0
  height 100%

body
  font-family PT Sans
  font-size $fontSize
  line-height 1.2
  word-wrap break-word

.main
  height 100%
  min-height 100%
  overflow-y scroll
  theme(color, $text)
  theme(background-color, $background)

.content-container
  position relative
  height 100%
  &:before
    content ''
    position absolute
    left 0
    right 0
    top 0
    bottom 0
    background url('~@/assets/header_bg.gif') left top repeat-x
    theme(filter, colorPair(none, invert(87%)))

.content-wrapper
  position relative
  margin auto
  min-height 100%
  min-width $minWidth
  max-width $maxWidth

.content-body
  display flex
  padding-bottom $footerHeight + $big

.content-menu
  menuContainer()

.content-sidebar
  sidebarContainer()

.content
  flex-grow 1

a
  theme(color, $activeText)
  text-decoration none
  &:hover
    theme(color, $activeHoverText)

</style>
