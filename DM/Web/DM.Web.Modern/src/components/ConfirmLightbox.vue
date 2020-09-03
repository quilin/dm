<template>
  <lightbox :name="name">
    <template slot="title">{{title}}</template>
    <template slot="controls">
      <button @click="accept" class="confirm-lightbox__accept">{{acceptText}}</button>
      <a @click="cancel">{{cancelText}}</a>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class ConfirmLightbox extends Vue {
  @Prop()
  private name!: string;

  @Prop()
  private title!: string;

  @Prop()
  private acceptText!: string;

  @Prop({ default: 'Отменить' })
  private cancelText!: string;

  private accept() {
    this.$emit('accepted');
  }

  private cancel() {
    this.$emit('canceled');
  }
}
</script>

<style scoped lang="stylus">
.confirm-lightbox
  &__accept
    margin-right $medium
</style>
