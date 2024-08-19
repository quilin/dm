<template>
  <div>
    <block-title class="title" @click="toggle">
      <slot name="title" />&nbsp;<the-icon
        :font="show ? IconType.CornerTop : IconType.CornerBottom"
      />
    </block-title>
    <div :class="{ list: true, collapsed: !show }" ref="content">
      <slot />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import { IconType } from "@/components/icons/iconType";

const props = defineProps<{ token: string }>();
const storageKey = computed(() => `__HideMenuModule_${props.token}__`);

const show = ref(localStorage.getItem(storageKey.value) !== false.toString());
const content = ref<HTMLElement | null>(null);

const toggle = () => {
  localStorage.setItem(storageKey.value, (show.value = !show.value).toString());
  if (show.value) {
    content.value!.style.height = "auto";
    const expectedHeight = content.value!.clientHeight;
    content.value!.style.height = "0";
    setTimeout(() => (content.value!.style.height = `${expectedHeight}px`), 0);
    setTimeout(() => (content.value!.style.height = "auto"), 200);
  } else {
    content.value!.style.height = `${content.value!.clientHeight}px`;
    setTimeout(() => (content.value!.style.height = "0"), 0);
  }
};
</script>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

.title
  cursor: pointer

.list
  overflow: hidden
  transition: height $animation-time

  &.collapsed
    height: 0
</style>
