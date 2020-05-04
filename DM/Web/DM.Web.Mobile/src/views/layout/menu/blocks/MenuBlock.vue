<template>
  <div class="menu-block">
    <h3><slot name="title"/></h3>
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
    .list
      margin-bottom $medium
      a
        text-decoration none
</style>
