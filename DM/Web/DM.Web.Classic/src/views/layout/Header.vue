<template>
  <div class="header">
    <div class="user-info">
      <div v-if="user"></div>
      <div v-else class="logo-text">Форумные ролевые игры</div>
      <router-link class="logo" :to="{name: 'home'}" />
      <a href="#">Вход</a>
      |
      <a href="#">Регистрация</a>
    </div>
    <div class="top-menu">
      <router-link class="top-menu-link" :to="{name: 'about'}">О проекте</router-link>
      <router-link class="top-menu-link" :to="{name: 'community'}">Сообщество</router-link>
      <router-link class="top-menu-link" :to="{name: 'fora'}">Форум</router-link>
      <router-link class="top-menu-link" :to="{name: 'rules'}">Правила</router-link>
      <router-link class="top-menu-link" :to="{name: 'chat'}">Чат</router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class DmHeader extends Vue {
  private user: any = null;
}
</script>

<style scoped lang="stylus">
.header
  display flex
  box-sizing border-box

  height 90px

  background transparent url('~@/assets/header_bg.gif') left top repeat-x

.user-info
  margin 0 20px 0 30px

.logo
  display block
  height 26px
  width 243px
  background transparent url('~@/assets/logo.gif') no-repeat
</style>
