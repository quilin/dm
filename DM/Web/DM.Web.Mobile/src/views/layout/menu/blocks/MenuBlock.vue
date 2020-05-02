<template>
  <div class="menu-block">
    <div class="title"><slot name="title"/></div>
    <div class="list"><slot /></div>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';

  @Component

  export default class MenuBlock extends Vue {

  }
</script>

<style scoped lang="stylus">
  .menu-block
    .title
      minorTitle()
    .list
      margin-bottom $medium
      a
        text-decoration none
</style>
