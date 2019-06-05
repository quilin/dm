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
  width $gridStep * 10
  height @width
  background-position center center
  background-repeat no-repeat
  background-size contain
  theme(background-image, colorPair(url('~@/assets/loader.gif'), none))
</style>
