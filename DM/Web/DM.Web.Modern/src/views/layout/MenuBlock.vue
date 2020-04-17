<template>
  <div :class="{ 'hidden': !show }">
    <div class="title" @click="toggle">
      <slot name="title" />
    </div>
    <div class="list">
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
  private get storageKey(): string {
    return `__HideLeftMenuModules__${this.token}__`;
  }

  private show: boolean = true;

  private toggle(): void {
    localStorage.setItem(this.storageKey, (this.show = !this.show).toString());
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
  .hidden &:after
    content ' '

.list
  .hidden &
    display none

</style>
