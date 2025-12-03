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
}>();
const primeItems = computed<MenuItem[]>(() =>
  props.items.map((item) => ({ label: item.label, command: item.command })),
);

const menu = useTemplateRef("menu");
</script>

<template>
  <span>
    <dm-icon
      class="menu-button"
      @click="menu!.toggle($event)"
      :font="IconType.Kebab"
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
          <dm-icon v-if="item.icon" :font="item.icon" />
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

.menu-button
  +Layout.square(Variables.$medium)
  +Themes.theme(background-color, Themes.$background)
  line-height: Variables.$grid-step * 3
  text-align: center
  padding: Variables.$tiny
  border-radius: Variables.$medium
  cursor: pointer

  &:hover
    +Themes.theme(background-color, Themes.$panel-background-hover)

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
    border-radius: Variables.$border-radius Variables.$border-radius 0 0
  &:last-child
    border-radius: 0 0 Variables.$border-radius Variables.$border-radius
  &:hover
    +Themes.theme(background, Themes.$panel-background-hover)
</style>
<style scoped lang="sass"></style>
