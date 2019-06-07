<template>
  <div>
    <div class="page-title">{{$route.params.id}} | Форум</div>
    <div class="moderators">
      Модераторы:
      <router-link
        v-if="moderators"
        v-for="moderator in moderators" :key="moderator.login"
        :to="{name: 'user', params: {login: moderator.login}}">
        {{moderator.login}}
      </router-link>
    </div>

    <div class="list">
      <div>Тема</div>
      <div>Дата</div>
      <div>Автор</div>
      <div><icon :font="IconType.CommentsNoUnread" /></div>
      <div>Последнее сообщение</div>
    </div>
    <loader v-if="!topics" class="topics-loader" />
    <template v-else-if="topics.length">
      <forum-topic v-for="topic in allTopics" :key="topic.id" :topic="topic" />
    </template>
    <div class="nothing" v-else>Еще не создано ни одной темы</div>
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import IconType from '@/components/iconType';
import { Topic } from '@/api/models/forum';
import { User } from '@/api/models/community';
import ForumTopic from './ForumTopic.vue';

const namespace: string = 'forum';

@Component({
  components: {
    ForumTopic,
  },
})
export default class ForumPage extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('moderators', { namespace })
  private moderators!: User[];

  @Getter('attachedTopics', { namespace })
  private attachedTopics!: Topic[];

  @Getter('topics', { namespace })
  private topics!: Topic[];

  private get allTopics(): Topic[] | null {
    if (this.attachedTopics === null || this.topics === null) {
      return null;
    }
    return this.attachedTopics.concat(this.topics);
  }

  @Action('selectForum', { namespace })
  private selectForum: any;

  @Action('fetchModerators', { namespace })
  private fetchModerators: any;

  @Action('fetchTopics', { namespace })
  private fetchTopics: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const id = this.$route.params.id;
    this.selectForum(id);
    this.fetchModerators(id);
    this.fetchTopics({ id, n: this.$route.params.n });
  }
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/forum/Grid'

.moderators
  header()
  a
    text-transform initial
    font-weight initial

.list
  grid()
  margin-top $medium
  theme(background-color, $blockBackground)
  & > *
    padding $minor

.topics-loader
  margin $medium auto

.nothing
  margin $medium
  text-align center
</style>
