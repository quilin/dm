<template>
  <button :disabled="loading || disabled" :class="{loading}" @click="click">
    <span><slot /></span>
  </button>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class ActionButton extends Vue {
  @Prop()
  private loading!: boolean;

  @Prop()
  private disabled?: boolean;

  private click() {
    this.$emit('click');
  }
}
</script>

<style scoped lang="stylus">
button.loading
  background-position center center
  background-image url('~@/assets/loader.gif')
  background-size $medium
  background-repeat no-repeat
  & span
    visibility hidden
</style>