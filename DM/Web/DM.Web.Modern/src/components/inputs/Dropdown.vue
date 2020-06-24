<template>
  <span :class="['container', popupShown && 'container_active']" ref="container">

    <span class="label" @click="popupShown = !popupShown">
      {{selectedOption.label}}
    </span>

    <portal to="popup" v-if="popupShown">
      <div class="options" :style="{top, left}">
        <div v-for="option in options" :key="option.value" class="option" @click="selectOption(option)">
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
  private value!: any;

  private selectedOption: DropdownOption = this.options.find(o => o.value == this.value) ?? this.options[0];

  private selectOption(option: DropdownOption) {
    this.popupShown = false;
    this.$emit('input', option.value);
  }
}
</script>

<style scoped lang="stylus">
.container
  display inline-block

  border 1px solid
  border-radius $tiny
  theme(border-color, $border)

.label
  display inline-block
  box-sizing border-box
  padding ($gridStep * 1.5) $medium ($gridStep * 1.5) $small

  background transparent url('~@/assets/dds-arrow.gif') right center no-repeat
  cursor pointer

.options
  position absolute

  margin-top $minor

  border 1px solid
  theme(border-color, $border)
  border-radius $borderRadius

.option
  padding ($gridStep * 1.5) $medium ($gridStep * 1.5) $small
  theme(background, $background)
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