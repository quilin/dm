<template>
  <div :class="{'collapsed': !show}">
    <div class="title" @click="toggle">
      <slot name="title" />
    </div>
    <div class="list" ref="content">
      <slot />
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class MenuBlock extends Vue {
  public $refs!: {
    content: HTMLElement;
  };

  @Prop()
  private token!: string;

  private get storageKey() {
    return `__HideLeftMenuModules__${ this.token }__`;
  }

  private show = true;

  private toggle() {
    localStorage.setItem(this.storageKey, (this.show = !this.show).toString());
    const content = this.$refs.content;
    if (this.show) {
      content.style.height = 'auto';
      const neededHeight = content.clientHeight;
      content.style.height = '0';
      setTimeout(() => content.style.height = `${ neededHeight }px`, 0);
      setTimeout(() => content.style.height = 'auto', 200);
    } else {
      content.style.height = `${ content.clientHeight }px`;
      setTimeout(() => content.style.height = '0', 0);
    }
  }

  private mounted() {
    const storedValue = localStorage.getItem(this.storageKey);
    if (storedValue === false.toString()) {
      this.show = false;
    }
  }
}
</script>

<style scoped lang="stylus">
.title
  header()
  cursor pointer

  &:after
    icon()
    content ' '

  .collapsed &:after
    content ' '

.list
  overflow hidden
  transition height .2s
  .collapsed &
    height 0

</style>
