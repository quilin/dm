<template>
  <div id="app" :class="`theme_${currentTheme}`">
    <div class="content" v-touch:swipe.left="showMenuBar">
      <div class="header">
        <router-view name="header"/>
      </div>
      <div class="page">
        <router-view name="page"/>
      </div>
      <div class="footer">
        <router-view name="footer"/>
      </div>
    </div>
    <div class="menu-bar" v-show="menuBarStatus">
      <router-view name="menuBar"/>
    </div>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import {Action, Getter} from 'vuex-class';

  @Component
  export default class DmApp extends Vue {
    @Getter('currentTheme')
    private currentTheme!: string;

    @Getter('menuBarStatus')
    private menuBarStatus!: boolean;

    @Action('fetchUser')
    private fetchUser: any;

    @Action('showMenuBar')
    private showMenuBar: any;

    private mounted(): void {
      this.fetchUser();
    }
  }
</script>

<style lang="stylus">
  *
    margin 0

  html, body, #app
    width 100%
    margin 0
    height 100%

  html
    overflow-x hidden

  body
    font-family PT Sans, sans-serif
    font-size $textFontSize
    line-height 1.2
    word-wrap break-word
    margin 0
    theme(color, $text)
    theme(background-color, $background)

  a
    theme(color, $activeText)
    text-decoration none

  #app
    position relative

    .content
      .header
        layoutNav(top, bottom)

      .page
        padding $layoutNav + $small $small

      .footer
        layoutNav(bottom, top)

    .menu-bar
      position: fixed;
      top: $big;
      left: $medium;
      right: 0;
      bottom: $layoutNav + $gridStep;
      theme(background-color, $background)
      box-shadow: -1px -1px 5px 1px rgba(0, 0, 0, 0.7);
    h1
      pageTitle()
    h2
      header()
    h3
      minorTitle()

    .button
      button()

</style>
