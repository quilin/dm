<template>
  <div class="header">
    <div class="user-info">
      <div class="logo-text">
        <template v-if="user"></template>
        <template v-else>Форумные ролевые игры</template>
      </div>
      <router-link class="logo" :to="{name: 'home'}" />
      <div class="user-actions">
        <template v-if="user"></template>
        <template v-else>
          <a href="#"><icon :font="IconType.User" /> Вход</a>
          |
          <a href="#">Регистрация</a>
        </template>
      </div>
    </div>
    <div class="top-menu">
      <router-link class="top-menu-link" :to="{name: 'about'}">О проекте</router-link>
      <router-link class="top-menu-link" :to="{name: 'community'}">Сообщество</router-link>
      <router-link class="top-menu-link" :to="{name: 'fora'}">Форум</router-link>
      <router-link class="top-menu-link" :to="{name: 'rules'}">Правила</router-link>
      <router-link class="top-menu-link" :to="{name: 'chat'}">Чат</router-link>
    </div>
    <div class="controls" @click="toggleTheme">
      Tumbler here!!!
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { mapActions } from 'vuex';
import IconType from '@/components/iconType';

@Component({
  methods: {
    ...mapActions(['toggleTheme']),
  },
})
export default class DmHeader extends Vue {
  private IconType: typeof IconType = IconType;
  private user: any = null;
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
  height 26px /// image size
  background transparent url('~@/assets/logo.gif') no-repeat
  theme(filter, colorPair(none, invert(87%)))

.logo-text
  theme(color, $highlightText)
  margin-bottom $minor

.top-menu
  display flex
  flex-grow 1
  margin 0 $minor
  padding $medium + $small 0

.top-menu-link
  margin-right $medium + $small
  font-size $medium
  letter-spacing 1px
  theme(color, $secondaryText)
  &:hover
    theme(color, $text)

.controls
  sidebarContainer()

</style>
