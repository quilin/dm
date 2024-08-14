<template>
  <div>
    <div v-if="topic">
      <div class="page-title">{{topic.title}}</div>
      <div class="description" v-html="topic.description"></div>
      <div class="data">
        <router-link :to="{name: 'user', params: {login: topic.author.login}}">{{topic.author.login}}</router-link>,
        <human-timespan :date="topic.created" />
        &nbsp;
        <like :entity="topic" @liked="addLike({ id: topic.id })" @unliked="deleteLike({ id: topic.id })" />
      </div>
      <router-link :to="{name: 'forum', params: {id: topic.forum.id}}">
        <icon :font="IconType.ArrowLeft" />
        Назад на форум "{{topic.forum.id}}"
      </router-link>
    </div>
    <loader v-else :big="true" />
    <router-view />
    <create-comment-form />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Topic } from '@/api/models/forum';
import IconType from '@/components/iconType';
import CreateCommentForm from './CreateCommentForm.vue';
import Like from '@/components/shared/Like.vue';

@Component({
  components: { CreateCommentForm, Like },
})
export default class TopicPage extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('forum/topic')
  private topic!: Topic;

  @Action('forum/selectTopic')
  private selectTopic: any;

  @Action('forum/markTopicAsRead')
  private markTopicAsRead: any;

  @Getter('forum/selectedTopic')
  private selectedTopic!: string | null;

  @Action('forum/deleteTopicLike')
  private deleteLike: any;

  @Action('forum/addTopicLike')
  private addLike: any;

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    const id = this.$route.params.id;

    await this.selectTopic({ id });

    if (this.topic.unreadCommentsCount) {
      this.markTopicAsRead({ id });
    }
  }
}
</script>

<style scoped lang="stylus">
.data
  margin $small 0
  theme(color, $secondaryText)
</style>
