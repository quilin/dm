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

    <div class="topicsList">
      <div class="topicsList-header">Тема</div>
      <div class="topicsList-header">Дата</div>
      <div class="topicsList-header">Автор</div>
      <div class="topicsList-header"><icon :font="IconType.CommentsNoUnread" /></div>
      <div class="topicsList-header">Последнее сообщение</div>
      <template v-if="topics" v-for="topic in topics">
        <router-link :to="{name: 'topic', params: {id: topic.id}}" class="topicList-link">
          {{topic.title}}
          <div class="topicList-description" v-html="topic.description"></div>
        </router-link>
        <div>{{topic.created.substr(0, 10)}}</div>
        <div>
          <router-link :to="{name: 'user', params: {login: topic.author.login}}">{{topic.author.login}}</router-link>
        </div>
        <div>{{topic.unreadCommentsCount}}</div>
        <div>
          <router-link
            v-if="topic.lastComment"
            :to="{name: 'user', params: {login: topic.author.login}}">
            {{topic.lastComment.author.login}},
            {{topic.lastComment.created.substr(0, 10)}}
          </router-link>
        </div>
      </template>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import IconType from '@/components/iconType';
import { Topic } from '@/api/models/forum';
import User from '@/api/models/community/user';

const namespace: string = 'forum';

@Component({})
export default class Forum extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('moderators', { namespace })
  private moderators!: User[];

  @Getter('topics', { namespace })
  private topics!: Topic[];

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
    this.fetchTopics({ id: this.$route.params.id, page: this.$route.params.page });
  }
}
</script>

<style scoped lang="stylus">
.title
  pageTitle()

.moderators
  header()
  a
    text-transform initial

.topicsList
  display grid
  margin-top $medium
  grid-template-columns \[title\] 40% \[date\] 11% \[author\] 14% \[count\] auto \[lastComment\] 26%
  justify-items stretch
  align-items stretch

  & > *
    padding $small $minor
    border-bottom 1px solid
    theme(border-bottom-color, $border)
    &.topicsList-header
      padding $minor
      theme(background-color, $blockBackground)

.topicList-link
  display block
  &:hover
    theme(background-color, $blockHoverBackground)

.topicList-description
  secondary()
</style>
