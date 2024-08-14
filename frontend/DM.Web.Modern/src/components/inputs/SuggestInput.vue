<template>
  <span class="container" ref="container">

    <input type="text" class="input" :value="value"
      :disabled="disabled"
      :readonly="readonly"
      :size="size"
      :maxlength="maxlength"
      @input="$emit('input', $event.target.value)"
      @focus="popupShown = true" />

    <portal to="popup" v-if="popupShown">
      <div class="options options_active" :style="{top, left}">
        <div v-for="suggestion in suggestions" :key="suggestion.value" class="option" @click.stop="selectSuggestion(suggestion)">
          <span class="option__label">{{suggestion.value}}</span>
          <div v-if="suggestion.description" class="option__description">{{suggestion.description}}</div>
        </div>
      </div>
    </portal>

  </span>
</template>

<script lang="ts">
import PopupBase from './../popupBase';
import { Component, Prop } from 'vue-property-decorator';

interface Suggestion {
  value: any;
  description?: string;
}

@Component({})
export default class SuggestInput extends PopupBase {
  @Prop()
  private disabled?: boolean;

  @Prop()
  private readonly?: boolean;

  @Prop()
  private size?: number;

  @Prop()
  private maxlength?: number;

  @Prop()
  private value!: any;

  @Prop()
  private suggestions!: Suggestion[];

  private selectSuggestion(suggestion: Suggestion): void {
    this.$emit('input', suggestion.value);
    this.popupShown = false;
  }
}
</script>

<style scoped lang="stylus">
.container
  position relative
  display inline-block

.input
  padding-right $medium
  background url('~@/assets/dds-arrow.gif') right center no-repeat !important
  theme(background-color, $background)

.options
  display none
  position absolute
  min-width 100%

  top 100%
  margin-top $minor

  border 1px solid
  theme(border-color, $border)
  border-radius $borderRadius
  theme(background-color, $background)

  &_active
    display block

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