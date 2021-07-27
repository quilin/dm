<template>
  <div>
    <div class="controls">TODO: элементы управления</div>
    <textarea :value="value" :disabled="disabled" @input="$emit('input', $event.target.value)" />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class TextArea extends Vue {
  @Prop()
  private value!: string;

  @Prop()
  private disabled!: boolean;

  private textUpdate(evt: any): void {
    this.$emit('input', evt.target.value);
  }
}
</script>

<style lang="stylus" scoped>
.controls
  secondary()

textarea
  display block
  height $large
  width 100%
  min-width $gridStep * 100
  box-sizing border-box
</style>
