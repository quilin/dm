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
  private text!: string

  private textUpdate(): void {
    this.$emit('input', this.text)
  }
}
</script>

<style lang="stylus" scoped>
textarea
  height $large
  width 100%
  box-sizing border-box
</style>
