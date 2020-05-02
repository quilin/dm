<template>
  <div class="footer-icons">
    <slot/>
    <icon :font="IconType.Sidebar" @click.native="toggleMenuBar"/>
  </div>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import IconType from '@/components/iconType';
  import {Action} from 'vuex-class';

  @Component
  export default class FooterDefault extends Vue {
    private IconType: typeof IconType = IconType;

    @Action('toggleMenuBar')
    private toggleMenuBar: any;
  }
</script>

<style scoped lang="stylus">
  .footer-icons
    footerIcons()
</style>
