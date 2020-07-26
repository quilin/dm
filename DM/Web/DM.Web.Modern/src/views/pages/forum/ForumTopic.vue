<template>
  <div class="row" :class="{ closed: topic.closed, attached: topic.attached }">
    <router-link :to="{name: 'topic', params: {id: topic.id}}" class="link">
      <icon v-if="topic.attached" :font="IconType.Attached" />
      <icon v-if="topic.closed" :font="IconType.Closed" />
      {{topic.title}}
      <div class="description" v-html="topic.description"></div>
      <div v-if="topic.unreadCommentsCount" class="unread">
        <icon :font="IconType.CommentsUnread" />{{topic.unreadCommentsCount}}
      </div>
    </router-link>
    <div>{{moment(topic.created).format("DD.MM.YYYY")}}</div>
    <div>
      <router-link :to="{name: 'user', params: {login: topic.author.login}}">{{topic.author.login}}</router-link>
    </div>
    <div>{{topic.commentsCount}}</div>
    <div>
      <router-link
        v-if="topic.lastComment"
        :to="{name: 'user', params: {login: topic.author.login}}">
        {{topic.lastComment.author.login}},
        <human-timespan :date="topic.lastComment.created" />
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Topic } from '@/api/models/forum';
import IconType from '@/components/iconType';
import moment from 'moment';

@Component({})
export default class ForumTopic extends Vue {
  private IconType: typeof IconType = IconType;
  private moment: any = moment;

  @Prop()
  private topic!: Topic;
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/forum/Grid'

.row
  grid($forumGridTemplate)
  &.closed
    opacity 0.7
    &.attached
      opacity initial

.link
  display block
  position relative
  &:hover
    theme(background-color, $panelHoverBackground)
  .attached &
    font-weight bold

.description
  secondary()

.unread
  position absolute
  right 100%
  top $tiny
  margin-right $minor
</style>
