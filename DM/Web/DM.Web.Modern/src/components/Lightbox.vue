<template>
  <portal to="lightbox">
    <modal :name="name" :adaptive="true" height="auto" width="452px">
      <div class="lightbox">
        <div class="page-title">
          <slot name="title"/>
        </div>
        <slot/>
        <div class="lightbox-controls">
          <slot name="controls"/>
        </div>
        <icon :font="IconType.Close" class="lightbox-close" @click.native="$modal.hide(name)"/>
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
  right $small - $tiny // font exception
  cursor pointer
  &:hover
    theme(color, $activeText)

.lightbox-controls
  margin $medium (-($lightboxOffset)) 0
  padding $controlsOffset $lightboxOffset
  theme(background-color, $controlBackground)

  & input[type='button']
    margin-right $medium
</style>
