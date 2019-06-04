<template>
  <div :class="{ 'hidden': !show }">
    <div class="title" @click="toggle">
      <slot name="title" />
    </div>
    <div v-if="show" class="list">
      <slot />
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class MenuBlock extends Vue {
  @Prop()
  private token!: string;
  private get cookieName(): string {
    return `__HideLeftMenuModules__${this.token}__`;
  }

  private show: boolean = true;

  private toggle(): void {
    if (this.show) {
      this.saveCookie();
    } else {
      this.removeCookie();
    }
    this.show = !this.show;
  }

  private mounted() {
    if (document.cookie.indexOf(this.cookieName) !== -1) {
      this.show = false;
    }
  }

  private saveCookie(): void {
    this.setCookie(new Date(new Date().getFullYear() + 10, 1, 1));
  }

  private removeCookie(): void {
    this.setCookie(new Date(0));
  }

  private setCookie(expires: Date): void {
    document.cookie = `${this.cookieName}=${this.cookieName};path=/;expires=${expires.toUTCString()}`;
  }
}
</script>

<style lang="stylus">
@import '~@/styles/Fonts'
@import '~@/styles/Themes'
@import '~@/styles/Variables'

.title
  header()
  theme(color, $highlightText)
  margin $gridStep * 5 0 $gridStep
  cursor pointer
  &:after
    icon()
    content ' '
  .hidden &:after
    content ' '
</style>
