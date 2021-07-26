<template>
  <div class="progress">
    <div class="progress-scale" :style="{width: `${current / goal * 100}%`}" />
    <div class="progress-text"><slot /></div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class ProgressBar extends Vue {
  @Prop()
  private current!: number;

  @Prop()
  private goal!: number;
}
</script>

<style scoped lang="stylus">
.progress
  position relative
  overflow hidden
  margin $small 0
  padding $minor

  theme(background-color, $panelBackground)
  transition background-color $animationTime
  border-radius $borderRadius

.progress-scale
  position absolute
  top 0
  bottom 0
  left 0
  theme(background-color, $panelHighlightBackground)
  transition width $animationTime, background-color $animationTime

.progress-text
  position relative
  theme(color, $text)
</style>