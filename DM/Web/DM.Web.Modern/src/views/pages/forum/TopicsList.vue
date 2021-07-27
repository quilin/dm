<template>
  <div>

    <paging v-if="paging"
      :paging="paging"
      :to="{name: 'forum', params: $route.params}" />

    <div class="list">
      <div>Тема</div>
      <div>Дата</div>
      <div>Автор</div>
      <div><icon :font="IconType.CommentsNoUnread" /></div>
      <div>Последнее сообщение</div>
    </div>

    <loader v-if="loading" :big="true" />
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
import { Paging } from '@/api/models/common';
import { Topic } from '@/api/models/forum';

import ForumTopic from './ForumTopic.vue';

@Component({
  components: {
    ForumTopic,
  },
})
export default class TopicsList extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('forum/topicsPaging')
  private paging!: Paging | null;

  @Getter('forum/attachedTopics')
  private attachedTopics!: Topic[];

  @Getter('forum/topics')
  private topics!: Topic[];

  private get loading(): boolean {
    return this.paging === null ||
      this.paging.pages > 0 && this.topics.length === 0;
  }

  private get allTopics(): Topic[] | null {
    if (this.attachedTopics === null || this.topics === null) {
      return null;
    }
    return this.attachedTopics.concat(this.topics);
  }

  @Action('forum/fetchTopics')
  private fetchTopics: any;

  @Watch('$route')
  private onRouteChange(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    this.fetchTopics(this.$route.params);
  }
}
</script>

<style lang="stylus" scoped>
@import '~@/views/pages/forum/Grid'

.list
  gridHead($forumGridTemplate)
  margin-top $medium

.nothing
  margin $medium
  text-align center
</style>
