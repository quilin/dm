<script setup lang="ts">
import { useUiStore, useUserStore } from "@/stores";
import { onMounted, watch } from "vue";
import { ModalsContainer } from "vue-final-modal";
import TheHeader from "@/views/layout/TheHeader.vue";
import TheFooter from "@/views/layout/TheFooter.vue";

const uiStore = useUiStore();
const userStore = useUserStore();

watch(
  () => [uiStore.theme],
  (value, oldValue) => {
    const html = document.querySelector("html")!;
    html.classList.remove(`theme_${oldValue}`);
    html.classList.add(`theme_${value}`);
  },
  { immediate: true },
);

onMounted(userStore.fetchUser);
</script>

<template>
  <div id="app">
    <div class="main" ref="scroll">
      <div class="content-container">
        <div class="content-wrapper">
          <the-header />
          <div class="content-body">
            <div class="content-menu">
              <router-view name="menu" />
            </div>
            <div class="content">
              <router-view name="page" />
            </div>
            <div class="content-sidebar">
              <router-view name="sidebar" />
            </div>
          </div>
        </div>
        <the-footer />
      </div>
    </div>
    <modals-container />
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"
@use "@/assets/styles/Themes"

.main
  height: 100%
  min-height: 100%
  overflow-y: scroll
  +Themes.theme(background-color, Themes.$background)

.content-container
  position: relative
  height: 100%
  min-width: Layout.$min-width
  &:before
    content: ''
    position: absolute
    left: 0
    right: 0
    top: 0
    bottom: 0
    background: url('@/assets/images/header_bg.gif') left top repeat-x
    +Themes.theme(filter, Themes.color-pair(none, invert(87%)))

.content-wrapper
  position: relative
  margin: auto
  min-height: 100%
  min-width: Layout.$min-width
  max-width: Layout.$max-width

.content-body
  display: flex
  line-height: Variables.$grid-step * 5
  padding-bottom: Layout.$footer-height + Variables.$big

.content-menu
  +Layout.menu-container()

.content
  +Layout.content-container()

.content-sidebar
  +Layout.sidebar-container()
</style>
