<template>
  <div class="header-block">
    <div class="title">
      <slot name="title"/>
    </div>
    <div class="subtitle">
      <slot name="subtitle"/>
    </div>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';

  @Component
  export default class Header extends Vue {

  }
</script>

<style scoped lang="stylus">
  .header-block
    display flex
    flex-direction column
    align-items center
    justify-content center
    height 100%

    .title
      font-size $titleFontSize
      theme(color, $text)

    .subtitle
      font-size $secondaryFontSize
      theme(color, $secondaryText)
</style>
