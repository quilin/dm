<template>
  <div class="row" :class="{ closed: topic.closed, attached: topic.attached }">
    <router-link
      :to="{ name: 'topic', params: { id: topic.id, n: topic.commentsCount - topic.unreadCommentsCount }}"
      class="title"
    >
      <icon v-if="topic.attached" :font="IconType.Attached" />
      <icon v-if="topic.closed" :font="IconType.Closed" />
      {{topic.title}}
    </router-link>
    <div>{{moment(topic.created).format("DD.MM.YYYY")}}</div>
    <div>
      <user-link :user="topic.author" />
    </div>
    <div>
      {{topic.commentsCount}}
      <span class="unread" v-if="topic.unreadCommentsCount">(+{{topic.unreadCommentsCount}})</span>
    </div>
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
  &:hover
    theme(background-color, $panelHoverBackground)
  &.closed
    opacity 0.7
    &.attached
      opacity initial

.unread
  font-weight bold

.title
  display block
  position relative
  .attached &
    font-weight bold
</style>
