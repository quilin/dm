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
    <portal-target name="lightbox"></portal-target>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import PortalVue from 'portal-vue';
import DmFooter from '@/views/layout/Footer.vue';
import DmHeader from '@/views/layout/Header.vue';

Vue.use(PortalVue);

@Component({
  components: {
    DmHeader,
    DmFooter,
  },
})
export default class DmApp extends Vue {
  @Getter('currentTheme')
  private currentTheme!: string;

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
  height 100%

body
  font-family PT Sans
  font-size $fontSize
  line-height 1
  word-wrap break-word

.v--modal
  theme(background-color, $background)
.v--modal-overlay
  theme(background-color, $overlayBackground)

.main
  height 100%
  min-height 100%
  overflow-y scroll
  theme(color, $text)
  theme(background-color, $background)
  transition color, background-color $animationTime

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
  margin-left 0
  margin-right $big

a
  theme(color, $activeText)
  text-decoration none
  transition all $animationTime
  cursor pointer
  &:hover
    theme(color, $activeHoverText)

.page-title
  pageTitle()

.content-title
  header()

.content-minor-title
  minorTitle()
</style>
