<template>
  <div id="app" :class="`theme_${theme}`">
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
      <modals-container />
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUiStore, useUserStore } from "@/stores";
import { onMounted } from "vue";
import { ModalsContainer } from "vue-final-modal";
import TheHeader from "@/views/layout/TheHeader.vue";
import TheFooter from "@/views/layout/TheFooter.vue";
import { storeToRefs } from "pinia";

const { theme } = storeToRefs(useUiStore());
const { fetchUser } = useUserStore();

onMounted(fetchUser);
</script>

<style scoped lang="sass">
.main
  height: 100%
  min-height: 100%
  overflow-y: scroll
  +theme(background-color, $background)
  +theme(color, $text)
  transition: color $animation-time, background-color $animation-time

.content-container
  position: relative
  height: 100%
  min-width: $min-width
  &:before
    content: ''
    position: absolute
    left: 0
    right: 0
    top: 0
    bottom: 0
    background: url('@/assets/images/header_bg.gif') left top repeat-x
    +theme(filter, color-pair(none, invert(87%)))
    transition: filter $animation-time

.content-wrapper
  position: relative
  margin: auto
  min-height: 100%
  min-width: $min-width
  max-width: $max-width

.content-body
  display: flex
  padding-bottom: $footer-height + $big

.content-menu
  +menu-container()

.content
  +content-container()

.content-sidebar
  +sidebar-container()
</style>
