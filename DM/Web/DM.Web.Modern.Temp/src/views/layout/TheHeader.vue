<template>
  <div class="header">
    <div class="user-info">
      <div class="logo-text">
        <template v-if="user">
          Добро пожаловать,
          <router-link :to="{ name: 'profile', params: { login: user.login } }">
            <the-icon :font="IconType.UserSettings" />
            {{ user.login }}
          </router-link>
        </template>
        <template v-else>Форумные ролевые игры</template>
      </div>
      <router-link class="logo" :to="{ name: 'home' }" />
      <div class="user-actions">
        <template v-if="user">
          <router-link :to="{ name: 'messenger' }" :class="{ unread }">
            <template v-if="unread">{{ unread }}</template>
            <the-icon
              :font="
                unread ? IconType.MessagesUnread : IconType.MessagesNoUnread
              "
            />
            Сообщения
          </router-link>
          |
          <a @click="signOut"><the-icon :font="IconType.Logout" /> Выйти</a>
        </template>
        <template v-else>
          <a @click="openLogin"><the-icon :font="IconType.User" /> Вход</a>
          |
          <a>Регистрация</a>
        </template>
      </div>
    </div>
    <div class="top-menu">
      <router-link class="link" :to="{ name: 'about' }">О проекте</router-link>
      <router-link class="link" :to="{ name: 'community' }"
        >Сообщество</router-link
      >
      <router-link class="link" :to="{ name: 'rules' }">Правила</router-link>
      <router-link class="link" :to="{ name: 'chat' }">Чат</router-link>
      <router-link
        v-if="user"
        class="link create"
        :to="{ name: 'create-game' }"
      >
        <the-icon :font="IconType.Add" />
        Новая игра
      </router-link>
    </div>
    <div class="controls">
      <!--      <notifications v-if="user" />-->
      <span @click="toggleTheme">Switch theme</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore, useUiStore } from "@/stores";
import { IconType } from "@/components/icons/iconType";
import { useModal } from "vue-final-modal";
import LoginForm from "@/views/account/LoginForm.vue";
import { storeToRefs } from "pinia";

const { toggleTheme } = useUiStore();
const userStore = useUserStore();
const { user, unreadConversations: unread } = storeToRefs(userStore);
const { signOut } = userStore;

const { open: openLogin, close: closeLogin } = useModal({
  component: LoginForm,
  attrs: {
    onLogin() {
      closeLogin();
    },
  },
});
</script>

<style scoped lang="sass">
.header
  display: flex
  box-sizing: border-box

  padding: $small 0
  height: 90px /// image size

  background-position: left top
  background-repeat: repeat-x

.user-info
  +menu-container()
  white-space: nowrap
  cursor: default

.logo
  display: block
  margin-bottom: $tiny
  height: 26px /// image size
  background: transparent url('@/assets/images/logo.gif') no-repeat
  +theme(filter, color-pair(none, invert(87%)))
  transition: filter $animation-time

.logo-text
  margin-bottom: $minor
  +theme(color, $highlight-text)

.unread
  font-weight: bold
  +theme(color, $positive-text)
  &:hover
    +theme(color, $active-text-hover)

.top-menu
  padding: $medium + $small 0
  +content-container()

.link
  margin-right: $medium + $small
  font-size: $text-font-size
  letter-spacing: 1px
  +theme(color, $secondary-text)

  &.router-link-active
    font-weight: bold
  &:hover
    +theme(color, $text)
  &.create
    padding: $minor + $tiny $small
    border-radius: $border-radius
    +theme(background, $panel-background)
    +theme(border, $panel-background, 1px solid)
    &.router-link-exact-active
      font-weight: normal
    &:hover
      +theme(border-color, $border)

.controls
  +sidebar-container()
</style>
