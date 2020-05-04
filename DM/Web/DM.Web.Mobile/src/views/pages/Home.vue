<template>
  <div class="home">
    <div class="user">
      <div v-if="user" class="user-info">

      </div>
      <div v-else class="login">
        <router-link :to="{name: 'UserLogin'}">Войти</router-link>
        <router-link :to="{name: 'UserRegister'}">Зарегистрироваться</router-link>
      </div>
    </div>
    <div class="news">
      <h2>Последние новости</h2>
    </div>
    <div class="buttons">
      <router-link :to="{name: 'PageAbout'}">О проекте</router-link>
      <router-link :to="{name: 'PageRules'}">Сообщество</router-link>
      <router-link :to="{name: 'PageCommunity'}">Правила</router-link>
      <router-link :to="{name: 'Chat'}">Чат</router-link>
    </div>
    <div class="content">
      Проект DungeonMaster.ru посвящен всем, кто не равнодушен к ролевым играм и системе Dungeons&Dragons в особенности.
      Если Вы хотите отправиться в небольшое приключение - смело присоединяйтесь к игре либо создайте свою собственную.
      Вести игроков сквозь хитросплетения сюжета своего модуля или непосредственно окунуться в виртуальную реальность -
      выбор за Вами!
    </div>
  </div>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from 'vue-property-decorator';
  import {Action, Getter} from 'vuex-class';
  import {User} from '@/api/models/community';


  @Component
  export default class Home extends Vue {
    @Getter('user')
    private user!: User | null;

    @Action('toggleTheme')
    private toggleTheme: any;

    @Action('signOut')
    private signOut: any;
  }
</script>

<style scoped lang="stylus">
  .home
    > div
      margin-bottom $medium

  .login
    display grid
    grid-template-columns 1fr
    grid-gap $small

    a
      button()

  .buttons
    display grid
    grid-template-columns repeat(2, 1fr)
    grid-gap $small

    a
      button()
</style>
