<script setup lang="ts">
import { Menu } from "primevue";
import { IconType } from "@/components/ui-kit/iconType";
import { computed, useTemplateRef } from "vue";
import type { MenuItem } from "primevue/menuitem";

export type DmMenuItem = {
  label: string;
  icon?: IconType;
  command?: () => any;
};
const props = defineProps<{
  items: DmMenuItem[];
  loading?: boolean;
}>();
const primeItems = computed<MenuItem[]>(() =>
  props.items.map((item) => ({
    key: item.label,
    label: item.label,
    command: item.command,
    dmIcon: item.icon,
  })),
);

const menu = useTemplateRef("menu");
</script>

<template>
  <span>
    <dm-icon-button
      :loading="loading"
      :icon="IconType.Kebab"
      @click="menu!.toggle($event)"
    />
    <Menu
      ref="menu"
      popup
      :model="primeItems"
      :pt="{
        root: 'menu-root',
        list: 'menu-list',
        item: 'menu-item',
      }"
    >
      <template #item="{ item }">
        <li>
          <dm-icon v-if="item.dmIcon" :font="item.dmIcon" />
          {{ item.label }}
        </li>
      </template>
    </Menu>
  </span>
</template>

<style lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"
@use "@/assets/styles/Layout"

.p-connected-overlay-enter-active
  animation: demo-overlay-in Variables.$animation-time ease-out

.p-connected-overlay-leave-active
  animation: demo-overlay-out Variables.$animation-time ease-in

@keyframes demo-overlay-in
  from
    opacity: 0
    transform: translateY(10%)

@keyframes demo-overlay-out
  to
    opacity: 0
    transform: translateY(10%)

.menu-root
  +Themes.theme(background, Themes.$background)
  +Themes.theme(border, Themes.$border, 1px solid)
  border-radius: Variables.$border-radius

.menu-list
  list-style: none

.menu-item
  padding: (Variables.$minor+1) Variables.$small
  +Themes.theme(background, Themes.$background)
  cursor: pointer

  &:first-child
    border-top-left-radius: Variables.$border-radius
    border-top-right-radius: Variables.$border-radius
  &:last-child
    border-bottom-left-radius: Variables.$border-radius
    border-bottom-right-radius: Variables.$border-radius
  &:hover
    +Themes.theme(background, Themes.$panel-background-hover)
</style>
<style scoped lang="sass"></style>
