<template>
  <span class="icon" v-html="charCode"></span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import IconType from './iconType';

@Component({})
export default class Icon extends Vue {
  @Prop()
  private font!: IconType;

  private get charCode(): string {
    return `&#x${this.font.toString(16)};`;
  }
}
</script>

<style scoped lang="stylus">
.icon
  icon()
</style>
