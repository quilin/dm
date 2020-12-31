<template>
  <div class="row" :class="{ closed: topic.closed, attached: topic.attached }">
    <router-link
      :to="{ name: 'topic', params: { id: topic.id, n: topic.commentsCount - topic.unreadCommentsCount }}"
      class="link">
      <icon v-if="topic.attached" :font="IconType.Attached" />
      <icon v-if="topic.closed" :font="IconType.Closed" />
      {{topic.title}}
      <div v-if="topic.unreadCommentsCount" class="unread">
        <icon :font="IconType.CommentsUnread" />{{topic.unreadCommentsCount}}
      </div>
    </router-link>
    <div>{{moment(topic.created).format("DD.MM.YYYY")}}</div>
    <div>
      <user-link :user="topic.author" />
    </div>
    <div>{{topic.commentsCount}}</div>
    <div>
      <template v-if="topic.lastComment">
        <user-link :user="topic.lastComment.author" />,
        <router-link :to="{name: 'topic', params: { id: topic.id, n: topic.commentsCount }}">
          <human-timespan :date="topic.lastComment.created" />
        </router-link>
      </template>
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

.unread
  position absolute
  right 100%
  top 6px
  margin-right $minor
</style>
