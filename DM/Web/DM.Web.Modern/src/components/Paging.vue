<template>
  <div class="paging" v-if="hasPages">
    <router-link
      v-for="(link, index) in links"
      :key="index"
      :to="link.to"
      :class="{active: link.isActive}"
      @click.native="prematureUpdate(link.n)">
      <icon v-if="link.icon" :font="link.icon" />
      <template v-else>{{link.n}}</template>
    </router-link>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Paging } from '@/api/models/common';
import IconType from './iconType';

interface PageLink {
  to: any;
  isActive: boolean;
  icon: IconType | null;
  n: number;
}

@Component({})
export default class PagingComponent extends Vue {
  @Prop()
  private paging!: Paging;

  @Prop()
  private to!: any;

  private get hasPages(): boolean {
    return this.paging.pages > 1;
  }

  private get links(): PageLink[] {
    if (!this.hasPages) {
      return [];
    }

    const result: PageLink[] = [];
    const lowerBound: number = Math.max(this.paging.current - 3, 1);
    const upperBound: number = Math.min(this.paging.current + 3, this.paging.pages);
    for (let i = lowerBound; i <= upperBound; ++i) {
      let icon: IconType | null;
      let entityNumber: number;
      if (i === lowerBound && lowerBound > 1) {
        icon = IconType.Backward;
        entityNumber = 1;
      } else if (i === upperBound && upperBound < this.paging.pages) {
        icon = IconType.Forward;
        entityNumber = (this.paging.pages - 1) * this.paging.size + 1;
      } else {
        icon = null;
        entityNumber = (i - 1) * this.paging.size + 1;
      }

      result.push({
        n: i,
        isActive: i === this.paging.current,
        icon,
        to: Object.assign({}, {
          name: this.to.name,
          params: Object.assign({}, this.to.params, {
            n: entityNumber,
          }),
        }),
      });
    }

    return result;
  }

  private prematureUpdate(pageNumber: number): void {
    this.paging.current = pageNumber;
  }
}
</script>

<style lang="stylus" scoped>
.paging
  text-align center
  & a
    display inline-block
    min-width $gridStep * 7
    text-align center
    padding $minor 0
    border-bottom 1px solid
    theme(border-bottom-color, $activeText)
    &.active
      border-bottom-width $minor
      padding-bottom 1px
      font-weight bold
    &:hover
      theme(border-bottom-color, $activeHoverText)
</style>
