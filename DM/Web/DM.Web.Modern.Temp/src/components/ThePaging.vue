<template>
  <div class="paging" v-if="hasPages">
    <router-link
      v-for="(link, index) in links"
      :key="index"
      :to="link.to"
      :class="{ active: link.isActive }"
      @click="prematureUpdate(link.n)"
    >
      <the-icon v-if="link.icon" :font="link.icon" />
      <template v-else>{{ link.n }}</template>
    </router-link>
  </div>
</template>

<script setup lang="ts">
import type { Paging } from "@/api/models/common";
import { computed, watch } from "vue";
import { IconType } from "@/components/icons/iconType";

interface PageLink {
  to: any;
  isActive: boolean;
  icon: IconType | null;
  n: number;
}

const props = defineProps<{ paging: Paging; to: any }>();

let localPaging: Paging;
watch(
  () => props.paging,
  (paging) => (localPaging = Object.assign({}, paging)),
  { immediate: true }
);

const prematureUpdate = (page: number) => (localPaging.current = page);

const hasPages = computed(() => localPaging.pages > 1);

const links = computed(() => {
  if (!hasPages.value) {
    return [];
  }

  const result: PageLink[] = [];
  const lowerBound: number = Math.max(localPaging.current - 3, 1);
  const upperBound: number = Math.min(
    localPaging.current + 3,
    localPaging.pages
  );
  for (let i = lowerBound; i <= upperBound; ++i) {
    let icon: IconType | null;
    let entityNumber: number;
    if (i === lowerBound && lowerBound > 1) {
      icon = IconType.Backward;
      entityNumber = 1;
    } else if (i === upperBound && upperBound < localPaging.pages) {
      icon = IconType.Forward;
      entityNumber = (localPaging.pages - 1) * localPaging.size + 1;
    } else {
      icon = null;
      entityNumber = (i - 1) * localPaging.size + 1;
    }

    result.push({
      n: i,
      isActive: i === localPaging.current,
      icon,
      to: {
        name: props.to.name,
        params: Object.assign({}, props.to.params, {
          n: entityNumber,
        }),
      },
    });
  }

  return result;
});
</script>

<style scoped lang="sass">
.paging
  text-align: center
  & a
    display: inline-block
    min-width: $grid-step * 7
    padding: $minor 0
    border-bottom: 1px solid
    +theme(border-bottom-color, $active-text)
    text-align: center

    &.active
      border-bottom-width: $minor
      padding-bottom: 1px
      font-weight: bold
    &:hover
      +theme(border-bottom-color, $active-text-hover)
</style>
