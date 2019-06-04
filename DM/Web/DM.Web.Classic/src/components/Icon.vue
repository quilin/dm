<template>
  <span :class="['icon', mod]" v-html="charCode"></span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import IconType from './IconType';

@Component({})
export default class Icon extends Vue {
  @Prop()
  private font!: IconType;

  @Prop()
  private mod!: string;

  private get charCode(): string {
    return `&#x${this.font.toString(16)};`;
  }
}
</script>

<style scoped lang="stylus">
@import '~@/styles/Fonts'

.icon
  icon()
</style>
