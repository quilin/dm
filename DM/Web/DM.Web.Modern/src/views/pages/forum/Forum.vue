<template>
  <div>
    <div class="forum-info">
      <div class="page-title">{{selectedForum}} | Форум</div>
      <a v-if="user && hasUnreadTopics" @click="markAllTopicsAsRead">Отметить всё прочитанным</a>
    </div>

    <div class="forum-info">
      <div v-if="moderators" class="moderators">
        Модераторы:
        <router-link
          v-for="moderator in moderators" :key="moderator.login"
          :to="{name: 'user', params: {login: moderator.login}}">
          {{moderator.login}}
        </router-link>
      </div>
      <create-topic v-if="canCreateTopic" />
    </div>

    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Route } from 'vue-router';

import { User } from '@/api/models/community';

import CreateTopic from './CreateTopic.vue';
import { Forum } from '@/api/models/forum';

@Component({
  components: {
    CreateTopic,
  },
})
export default class ForumPage extends Vue {
  @Getter('forum/selectedForum')
  private selectedForum!: string | null;

  @Getter('forum/moderators')
  private moderators!: User[];

  @Getter('user')
  private user!: User;

  @Getter('forum/forum')
  private forum!: Forum;

  @Action('forum/selectForum')
  private selectForum: any;

  @Action('forum/fetchModerators')
  private fetchModerators: any;

  @Action('forum/markAllTopicsAsRead')
  private markAllTopicsAsReadAction: any;

  private get hasUnreadTopics(): boolean {
    return this.forum && Boolean(this.forum.unreadTopicsCount);
  }

  private get canCreateTopic(): boolean {
    return this.user &&
      (this.selectedForum !== 'Новости проекта' ||
       this.user.roles.some((r: string) =>
         r === 'Administrator' || r === 'SeniorModerator'));
  }

  @Watch('$route')
  private onRouteChanged(newValue: Route, oldValue: Route): void {
    if (newValue.params.id !== oldValue.params.id) {
      this.fetchData();
    }
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const id = this.$route.params.id;
    this.selectForum({ id, router: this.$router });
    this.fetchModerators({ id });
  }

  private markAllTopicsAsRead() {
    this.markAllTopicsAsReadAction({ id: this.$route.params.id });
  }
}
</script>

<style scoped lang="stylus">
.forum-info
  display grid
  grid-template-columns auto auto
  align-items baseline

  & > *:nth-child(even)
    justify-self end

.moderators
  header()
  margin $small 0
  a
    text-transform initial
    font-weight initial
</style>
