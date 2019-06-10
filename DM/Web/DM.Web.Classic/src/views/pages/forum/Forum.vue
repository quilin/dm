<template>
  <div>
    <div class="forum-info">
      <div class="page-title">{{selectedForum}} | Форум</div>
      <a v-if="user" href="javascript:void(0)">Отметить всё прочитанным</a>
    </div>

    <div class="forum-info">
      <div class="moderators">
        Модераторы:
        <router-link
          v-if="moderators"
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

const namespace: string = 'forum';

@Component({
  components: {
    CreateTopic,
  },
})
export default class ForumPage extends Vue {
  @Getter('selectedForum', { namespace })
  private selectedForum!: string | null;

  @Getter('moderators', { namespace })
  private moderators!: User[];

  @Getter('user')
  private user!: User;

  @Action('selectForum', { namespace })
  private selectForum: any;

  @Action('fetchModerators', { namespace })
  private fetchModerators: any;

  private get canCreateTopic(): boolean {
    return this.user &&
      (this.selectedForum !== 'Новости проекта' ||
       this.user.roles.some((r: string) =>
        r === 'Admin' || r === 'SeniorModerator'));
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
    this.selectForum(id);
    this.fetchModerators(id);
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
