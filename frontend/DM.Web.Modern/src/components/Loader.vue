<template>
  <div :class="{ 'loader': true, 'big-loader': big }">&nbsp;</div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class Loader extends Vue {
  @Prop()
  private big!: boolean;
}
</script>

<style scoped lang="stylus">
.loader
  square($medium)
  background-image url('~@/assets/loader.gif')

.big-loader
  square($big)
  margin $big auto
</style>
