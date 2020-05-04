<template>
  <div class="home">
    <div class="news">
      <h2>Последние новости</h2>
      <loader v-if="!news" />
      <div v-else-if="!news.length">
        Ничего нового!
      </div>
      <template v-else>
        <div v-for="article in news" :key="article.id">
          <TopicListBlock :article="article" />
        </div>
        <router-link :to="{name: 'forum', params: {id: 'Новости проекта'}}" class="news-rest">
          К остальным новостям <icon :font="IconType.Forward" />
        </router-link>
      </template>
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
  import {Component, Vue} from 'vue-property-decorator';
  import {Action, Getter} from 'vuex-class';
  import { Topic } from '@/api/models/forum';
  import IconType from '@/components/iconType';
  import TopicListBlock from "@/views/pages/forum/TopicListBlock.vue";
  @Component({
    components: {TopicListBlock}
  })
  export default class Home extends Vue {
    private IconType: typeof IconType = IconType;

    @Getter('news', { namespace: 'forum' })
    private news!: Topic[];

    @Action('fetchNews', { namespace: 'forum' })
    private fetchNews: any;

    private mounted(): void {
      this.fetchNews();
    }
  }
</script>

<style scoped lang="stylus">
  .home
    > div
      margin-bottom $big

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
