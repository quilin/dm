<template>
  <div id="app">
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
    <div class="sidebar" v-show="sidebarShow.status">
      <router-view name="sidebar"/>
    </div>
  </div>
</template>

<script>
  import {mapGetters} from "vuex";

  export default {
    name: 'app',
    created() {
      this.fetch()
    },
    computed: {
      ...mapGetters({
        sidebarShow: 'getInterfaceSidebarShow',
      })
    },
    watch: {
      $route: 'fetch'
    },
    methods: {
      fetch() {
      },
    },
  }
</script>

<style lang="less">
  @import "./assets/less/vars";

  @font-face {
    font-family: Iconic;
    src: url("./assets/fonts/DM-eco-webfont-clean.ttf") format('truetype'),
    url("./assets/fonts/DM-eco-webfont-clean.woff") format('woff'),
    url("./assets/fonts/DM-eco-webfont-clean.otf") format('opentype');
    font-weight: normal;
    font-style: normal;
  }

  @font-face {
    font-family: PT Sans, sans-serif;
    src: url('//fonts.googleapis.com/css?family=PT+Sans:400,700,400italic,700italic&subset=latin,cyrillic')
  }

  * {
    margin: 0;
    font-weight: normal;
  }

  strong {
    font-weight: bold;
  }

  html {
    height: 100%;
    overflow-x: hidden;
  }

  body {
    height: 100%;
    margin: 0;
    font-family: 'PT Sans', Helvetica, sans-serif;
    font-size: 16px;
    line-height: 1.2;
    color: @color-basic;
    background-color: @color-basic-l;
  }

  input, textarea {
    padding: 4px;
    border: 1px solid #ccc;

    font-family: 'PT Sans', Helvetica, sans-serif;
    font-size: 16px;

    outline: none;
    border-radius: 3px;
  }

  #app {
    height: 100%;
    position: relative;

    .content {
      width: 100vw;

      .header {
        height: 3rem;
        color: @color-basic-l;
        background-color: @color-basic;
        text-align: center;
      }

      .page {
        margin-bottom: 4rem;
      }

      .footer {
        position: fixed;
        bottom: 0;
        left: 0;
        width: 100vw;
        height: 3rem;
        border-top: @color-basic 1px solid;
        text-align: center;
      }
    }

    .sidebar {
      position: fixed;
      top: 3.2rem;
      left: 1rem;
      right: 0;
      bottom: 3.2rem;
      box-shadow: -1px -1px 5px 1px rgba(0, 0, 0, 0.7);
    }
  }
  .iconic {
    font-family: Iconic!important;
    font-weight: normal!important;
    -webkit-font-smoothing: antialiased;
  }
  .base-unselectable {
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    -o-user-select: none;
    user-select: none;
  }

  a {
    text-decoration: none;
    color: @color-link;
  }

</style>
