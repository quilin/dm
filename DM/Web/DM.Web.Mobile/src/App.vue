<template>
  <div id="app" :class="`theme_${currentTheme}`">
    <div class="content">
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
    import {Getter} from 'vuex-class';

    @Component
    export default class DmApp extends Vue {
        @Getter('currentTheme')
        private currentTheme!: string;

        @Getter('menuBarStatus')
        private menuBarStatus!: boolean;
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

</style>
