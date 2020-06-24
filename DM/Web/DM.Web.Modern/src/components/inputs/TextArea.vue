<template>
  <div>
    <div class="controls"></div>
    <textarea @input="textUpdate"></textarea>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class TextArea extends Vue {
  @Prop()
  private text!: string;

  private textUpdate(evt: any): void {
    this.$emit('input', evt.target.value);
  }
}
</script>

<style lang="stylus" scoped>
textarea
  display block
  height $large
  width 100%
  box-sizing border-box
</style>
