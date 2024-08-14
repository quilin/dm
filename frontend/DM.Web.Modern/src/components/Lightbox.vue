<template>
  <portal to="lightbox">
    <modal :name="name" :adaptive="true" height="auto" :width="width || 452">
      <div class="lightbox">
        <div class="page-title">
          <slot name="title" />
        </div>
        <slot />
        <div class="lightbox-controls">
          <slot name="controls" />
        </div>
        <icon :font="IconType.Close" class="lightbox-close" @click.native="$modal.hide(name)" />
      </div>
    </modal>
  </portal>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import IconType from '@/components/iconType';

@Component({})
export default class Lightbox extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private name!: string;

  @Prop()
  private width?: number | null;
}
</script>

<style lang="stylus">
$lightboxOffset = $medium + $small
$controlsOffset = $small + $minor

.lightbox
  padding 0 $lightboxOffset

.lightbox-close
  font-size $titleFontSize
  position absolute
  top $small
  right $small - $tiny // особенности шрифта вынуждают уточнить положение
  cursor pointer
  &:hover
    theme(color, $activeText)

.lightbox-controls
  display flex
  align-items baseline
  margin $medium (-($lightboxOffset)) 0
  padding $controlsOffset $lightboxOffset
  theme(background-color, $controlBackground)
</style>
