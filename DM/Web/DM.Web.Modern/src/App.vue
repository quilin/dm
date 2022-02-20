<template>
  <div id="app" :class="`theme_${theme}`">
    <div class="main" ref="scroll">
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
            </div>
          </div>
        </div>
        <dm-footer />
      </div>
      <portal-target name="lightbox" multiple />
      <portal-target name="popup" class="popup-container" multiple />
      <portal-target name="notifications" class="notifications-container" />
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import DmFooter from '@/views/layout/Footer.vue';
import DmHeader from '@/views/layout/Header.vue';

@Component({
  components: {
    DmHeader,
    DmFooter,
  },
})
export default class DmApp extends Vue {
  public $refs!: {
    scroll: HTMLElement;
  }

  @Getter('ui/theme')
  private theme!: string;

  @Action('fetchUser')
  private fetchUser: any;

  private mounted(): void {
    this.fetchUser();
  }
}
</script>

<style lang="stylus">
html, body, #app
  height 100%
  margin 0
  overflow hidden

body
  font-family PT Sans
  font-size $fontSize
  line-height 1.3
  word-wrap break-word

.v--modal
  theme(background-color, $background)
.v--modal-overlay
  theme(background-color, $overlayBackground)
  overflow auto
.v--modal-box
  margin-top $big
  margin-bottom $big

.main
  height 100%
  min-height 100%
  overflow-y scroll
  theme(background-color, $background)
  theme(color, $text)
  transition color $animationTime, background-color $animationTime

.content-container
  position relative
  height 100%
  min-width $minWidth
  &:before
    content ''
    position absolute
    left 0
    right 0
    top 0
    bottom 0
    background url('~@/assets/header_bg.gif') left top repeat-x
    theme(filter, colorPair(none, invert(87%)))
    transition filter $animationTime

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
  min-width 0 // fix inner overflow elements https://www.w3.org/TR/css-flexbox-1/#flex-common
  margin-left 0
  margin-right $big

a
  theme(color, $activeText)
  text-decoration none
  transition color $animationTime
  cursor pointer
  &:hover
    theme(color, $activeHoverText)

.page-title
  pageTitle()

.content-title
  header()

.content-minor-title
  minorTitle()

.popup-container
  position absolute
  top 0
  left 0
  z-index 1000
  theme(color, $text)

.notifications-container
  position absolute
  bottom 0
  right $medium
  z-index 1000

.quote
  padding $small
  theme(background-color, $quoteBackground)
  themeExtend(border-left, $minor solid, $quoteOutline)

.info-head
  display inline-block
  margin $medium 0 $small
  font-size $titleFontSize
  font-weight normal
  theme(color, $highlightText)
</style>
