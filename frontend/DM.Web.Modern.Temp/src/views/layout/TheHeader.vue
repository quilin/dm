<script setup lang="ts">
import { useUserStore, useUiStore } from "@/stores";
import { IconType } from "@/components/ui-kit/iconType";
import GuestActions from "@/views/layout/header/GuestActions.vue";
import PlayerActions from "@/views/layout/header/PlayerActions.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import messages from "@/views/layout/TheHeader.i18n";
import { useI18n } from "vue-i18n";
import DmButton from "@/components/ui-kit/DmButton.vue";

const uiStore = useUiStore();
const userStore = useUserStore();
const { t } = useI18n({ messages });
</script>

<template>
  <div class="header">
    <div class="user-info">
      <div class="logo-text">
        <template v-if="userStore.user">
          {{ t("welcome") }},
          <router-link
            :to="{ name: 'profile', params: { login: userStore.user.login } }"
          >
            <dm-icon :font="IconType.UserSettings" />
            {{ userStore.user.login }}
          </router-link>
        </template>
        <template v-else>{{ t("dm") }}</template>
      </div>
      <router-link class="logo" :to="{ name: 'home' }" />
      <player-actions v-if="userStore.user" />
      <guest-actions v-else />
    </div>
    <div class="top-menu">
      <router-link class="link" :to="{ name: 'about' }">{{
        t("about")
      }}</router-link>
      <router-link class="link" :to="{ name: 'community' }">{{
        t("community")
      }}</router-link>
      <router-link class="link" :to="{ name: 'rules' }">{{
        t("rules")
      }}</router-link>
      <router-link class="link" :to="{ name: 'chat' }">{{
        t("chat")
      }}</router-link>
      <router-link
        v-if="userStore.user"
        class="link create"
        :to="{ name: 'create-game' }"
      >
        <dm-icon :font="IconType.Add" />
        {{ t("newGame") }}
      </router-link>
    </div>
    <div class="controls">
      <!--      <notifications v-if="user" />-->
      <dm-button label="Switch theme" @click="uiStore.toggleTheme" /><br />
      <dm-button
        label="Switch language"
        @click="$i18n.locale = $i18n.locale == 'ru-RU' ? 'en-US' : 'ru-RU'"
      />
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"
@use "@/assets/styles/Themes"

.header
  display: flex
  box-sizing: border-box

  padding: Variables.$small 0
  height: 90px /// image size

  background-position: left top
  background-repeat: repeat-x

.user-info
  +Layout.menu-container()
  white-space: nowrap
  cursor: default

.logo
  display: block
  margin-bottom: Variables.$tiny
  height: 26px /// image size
  background: transparent url('@/assets/images/logo.gif') no-repeat
  +Themes.theme(filter, Themes.color-pair(none, invert(87%)))

.logo-text
  margin-bottom: Variables.$minor
  +Themes.theme(color, Themes.$highlight-text)

.unread
  font-weight: bold
  +Themes.theme(color, Themes.$positive-text)
  &:hover
    +Themes.theme(color, Themes.$active-text-hover)

.top-menu
  padding: Variables.$medium + Variables.$small 0
  +Layout.content-container()

.link
  margin-right: Variables.$medium + Variables.$small
  font-size: Variables.$text-font-size
  letter-spacing: 1px
  +Themes.theme(color, Themes.$secondary-text)

  &.router-link-active
    font-weight: bold
  &:hover
    +Themes.theme(color, Themes.$text)
  &.create
    padding: Variables.$minor + Variables.$tiny Variables.$small
    border-radius: Variables.$border-radius
    +Themes.theme(background, Themes.$panel-background)
    +Themes.theme(border, Themes.$panel-background, 1px solid)
    &.router-link-exact-active
      font-weight: normal
    &:hover
      +Themes.theme(border-color, Themes.$border)

.controls
  +Layout.sidebar-container()
</style>
