<template>
  <span :class="['container', popupShown && 'container_active']"
    ref="container" @click="popupShown = !popupShown">

    <span class="label">
      {{selectedOption.label}}
    </span>

    <portal to="popup" v-if="popupShown">
      <div class="options" :style="{top, left}">
        <div v-for="option in options" :key="option.label" class="option" @click="selectOption(option)">
          <span class="option__label">{{option.label}}</span>
          <div v-if="option.description" class="option__description">{{option.description}}</div>
        </div>
      </div>
    </portal>

  </span>
</template>

<script lang="ts">
import PopupBase from './../popupBase';
import { Component, Prop } from 'vue-property-decorator';

interface DropdownOption {
  value: any;
  label: string;
  description?: string;
}

@Component({})
export default class Dropdown extends PopupBase {
  @Prop()
  private options!: DropdownOption[];

  @Prop()
  private emptyLabel?: string;

  @Prop()
  private value!: any;

  private get selectedOption(): DropdownOption {
    return this.options.find(o => o.value == this.value) ??
      {value: this.value, label: this.emptyLabel} as DropdownOption ??
      this.options[0];
  }

  private selectOption(option: DropdownOption) {
    this.popupShown = false;
    this.$emit('input', option.value);
  }
}
</script>

<style scoped lang="stylus">
.container
  display inline-flex
  overflow hidden

  border 1px solid
  border-radius $borderRadius
  theme(border-color, $border)
  background transparent url('~@/assets/dds-arrow.gif') right center no-repeat

.label
  display block
  overflow hidden
  padding $inputPadding
  padding-right $medium

  cursor pointer
  white-space nowrap
  text-overflow ellipsis
  vertical-align baseline

.options
  position absolute
  overflow auto

  margin-top $minor
  max-height $gridStep * 30

  border 1px solid
  theme(border-color, $border)
  border-radius $borderRadius

.option
  padding ($gridStep * 1.5) $medium ($gridStep * 1.5) $small
  theme(background-color, $background)
  cursor pointer
  white-space nowrap

  &:hover
    theme(background-color, $ddsOptionHoverBackground)

  &__description
    margin-top $tiny
    secondary()

  &:first-child
    border-radius $borderRadius $borderRadius 0 0
  &:last-child
    border-radius 0 0 $borderRadius $borderRadius
</style>