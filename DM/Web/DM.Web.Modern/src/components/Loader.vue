<template>
  <div class="loader" :class="classMod"></div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class Loader extends Vue {
  @Prop()
  private classMod?: string;
}
</script>

<style scoped lang="stylus">
.loader
  square($medium)
  background-image url('~@/assets/loader.gif')
</style>
