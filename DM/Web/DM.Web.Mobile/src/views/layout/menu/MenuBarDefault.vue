<template>
  <MenuBar>
    <MenuFora/>
  </MenuBar>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import MenuBar from "./MenuBar.vue";
  import MenuFora from "./blocks/MenuFora.vue";

  @Component({
    components: {
      MenuBar,
      MenuFora,
    }
  })
  export default class MenuBarDefault extends Vue {

  }
</script>

<style scoped lang="stylus">

</style>
