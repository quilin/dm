<template>
  <div>
    <div class="title">{{$route.params.id}} | Форум</div>
    <div class="moderators">
      Модераторы:
      <router-link
        v-if="moderators"
        v-for="moderator in moderators" :key="moderator.login"
        :to="{name: 'user', params: {login: moderator.login}}">
        {{moderator.login}}
      </router-link>
    </div>

    <div class="list header">
      <div>Тема</div>
      <div>Дата</div>
      <div>Автор</div>
      <div><icon :font="IconType.CommentsNoUnread" /></div>
      <div>Последнее сообщение</div>
    </div>
    <forum-topic v-if="topics" v-for="topic in allTopics" :key="topic.id" :topic="topic" />
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import IconType from '@/components/iconType';
import { Topic } from '@/api/models/forum';
import User from '@/api/models/community/user';
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
    this.selectForum({ id: this.$route.params.id });
    this.fetchTopics({ id: this.$route.params.id, n: this.$route.params.n });
  }
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/forum/Grid'

.title
  pageTitle()

.moderators
  header()
  a
    text-transform initial

.list
  grid()
  margin-top $medium
  theme(background-color, $blockBackground)
  & > *
    padding $minor
</style>
