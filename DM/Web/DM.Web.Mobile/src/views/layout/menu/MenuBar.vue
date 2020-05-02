<template>
  <div class="menu">
    <div class="close">
      <icon :font="IconType.Close" @click.native="hideMenuBar"/>
    </div>
    <slot/>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import IconType from '@/components/iconType';
  import {Action} from 'vuex-class';

  @Component
  export default class MenuBar extends Vue {
    private IconType: typeof IconType = IconType;

    @Action('hideMenuBar')
    private hideMenuBar: any;
  }
</script>

<style scoped lang="stylus">
  .menu
    padding $small

    .close
      position absolute
      width $medium + $small
      height @width
      right $small

      .icon
        font-size $medium + $small

</style>
