<template>
  <div class="row" :class="{ closed: topic.closed, attached: topic.attached }">
    <router-link :to="{name: 'topic', params: {id: topic.id}}" class="link">
      <icon v-if="topic.attached" :font="IconType.Attached" />
      <icon v-if="topic.closed" :font="IconType.Closed" />
      {{topic.title}}
      <div class="description" v-html="topic.description"></div>
    </router-link>
    <div>{{topic.created.substr(0, 10).split('-').reverse().join('.')}}</div>
    <div>
      <router-link :to="{name: 'user', params: {login: topic.author.login}}">{{topic.author.login}}</router-link>
    </div>
    <div>{{topic.unreadCommentsCount}}</div>
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

@Component({})
export default class ForumTopic extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private topic!: Topic;
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/forum/Grid'

.row
  grid()
  &.closed
    opacity 0.7
    &.attached
      opacity initial
  & > *
    padding $minor

.link
  display block
  &:hover
    theme(background-color, $blockHoverBackground)
  .attached &
    font-weight bold

.description
  secondary()
</style>
