<template>
  <Footer>
    <router-link :to="{ name: 'Home' }">
      <icon :font="IconType.Home"/>
    </router-link>
  </Footer>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import IconType from '@/components/iconType';
  import Footer from "./Footer.vue";

  @Component({
    components: {
      Footer,
    },
  })
  export default class FooterDefault extends Vue {
    private IconType: typeof IconType = IconType;
  }
</script>

<style scoped lang="stylus">
  .footer-icons
    footerIcons()
</style>
