<template>
  <div class="header">
    <div class="user-info">
      <div class="logo-text">
        <template v-if="user">
          Добро пожаловать,
          <router-link :to="{ name: 'profile', params: { login: user.login } }">
            <icon :font="IconType.UserSettings" />
            {{user.login}}
          </router-link>
        </template>
        <template v-else>Форумные ролевые игры</template>
      </div>
      <router-link class="logo" :to="{name: 'home'}" />
      <div class="user-actions">
        <template v-if="user">
          <a href="javascript:void(0)" :class="{ unread }">
            <template v-if="unread">{{unread}} </template>
            <icon :font="unread ? IconType.MessagesUnread : IconType.MessagesNoUnread" /> Сообщения
          </a>
          |
          <a @click="signOut"><icon :font="IconType.Logout" /> Выйти</a>
        </template>
        <template v-else>
          <a @click="$modal.show('login')"><icon :font="IconType.User" /> Вход</a>
          |
          <a @click="$modal.show('register')">Регистрация</a>
        </template>
      </div>
    </div>
    <div class="top-menu content">
      <router-link class="link" :to="{name: 'about'}">О проекте</router-link>
      <router-link class="link" :to="{name: 'community'}">Сообщество</router-link>
      <router-link class="link" :to="{name: 'rules'}">Правила</router-link>
      <router-link class="link" :to="{name: 'chat'}">Чат</router-link>
      <router-link v-if="user" class="link create" :to="{name: 'create-game'}">
        <icon :font="IconType.Add" />
        Новая игра
      </router-link>
    </div>
    <div class="controls">
      <notifications v-if="user" />
      <span @click="toggleTheme">Switch theme</span>
    </div>

    <template v-if="!user">
      <login />
      <register />
    </template>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import IconType from '@/components/iconType';
import Login from './Login.vue';
import Notifications from '@/components/notifications/Notifications.vue';
import Register from '@/views/layout/Register.vue';

@Component({
  components: {
    Register,
    Notifications,
    Login,
  },
})
export default class DmHeader extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('user')
  private user!: User | null;

  @Getter('unreadConversations')
  private unread!: number;

  @Action('ui/toggleTheme')
  private toggleTheme: any;

  @Action('signOut')
  private signOut: any;
}
</script>

<style scoped lang="stylus">
.header
  display flex
  box-sizing border-box

  padding $small 0
  height 90px /// image size

  background-position left top
  background-repeat repeat-x

.user-info
  menuContainer()
  white-space nowrap
  cursor default

.logo
  display block
  margin-bottom $tiny
  height 26px /// image size
  background transparent url('~@/assets/logo.gif') no-repeat
  theme(filter, colorPair(none, invert(87%)))
  transition filter $animationTime

.logo-text
  theme(color, $highlightText)
  margin-bottom $minor

.unread
  theme(color, $positiveText)
  font-weight bold
  &:hover
    theme(color, $activeHoverText)

.top-menu
  padding $medium + $small 0

.link
  margin-right $medium + $small
  font-size $textFontSize
  letter-spacing 1px
  theme(color, $secondaryText)
  transition all $animationTime
  &.router-link-active
    font-weight bold
  &:hover
    theme(color, $text)
  &.create
    padding $minor + $tiny $small
    border-radius $borderRadius
    border 1px solid
    theme(background, $panelBackground)
    theme(border-color, $panelBackground)
    &.router-link-exact-active
      font-weight normal
    &:hover
      theme(border-color, $border)

.controls
  sidebarContainer()
</style>
