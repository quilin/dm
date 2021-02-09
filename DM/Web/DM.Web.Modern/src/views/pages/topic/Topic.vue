<template>
  <div>
    <div v-if="topic">
      <div class="page-title">{{topic.title}}</div>
      <div class="topic">
        <div class="description" v-html="topic.description"></div>
        <div class="topic__meta">
          <div>
            <user-link :user="topic.author" />,
            <human-timespan :date="topic.created" />
            &nbsp;
            <like :entity="topic" @liked="addLike({ id: topic.id })" @unliked="deleteLike({ id: topic.id })" />
          </div>
          <div class="topic__controls" v-if="topicEditable">
            <a class="topic-__control" @click="showEditForm">
              <icon :font="IconType.Edit" />
              Редактировать
            </a>
          </div>
        </div>
      </div>
      <router-link :to="{name: 'forum', params: {id: topic.forum.id}}">
        <icon :font="IconType.ArrowLeft" />
        Назад на форум "{{topic.forum.id}}"
      </router-link>
    </div>
    <loader v-else :big="true" />
    <topic-comments />
    <create-comment-form />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Topic } from '@/api/models/forum';
import IconType from '@/components/iconType';
import TopicComments from './TopicComments.vue';
import CreateCommentForm from './CreateCommentForm.vue';
import Like from '@/components/shared/Like.vue';
import { User } from '@/api/models/community';
import { userIsHighAuthority } from '@/api/models/community/helpers';

@Component({
  components: { CreateCommentForm, TopicComments, Like },
})
export default class TopicPage extends Vue {
  private IconType: typeof IconType = IconType;

  private editMode = false;

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

  @Getter('user')
  private user!: User | null;

  @Getter('forum/moderators')
  private moderators!: User[];

  private topicEditable(): boolean {
    return this.moderators.some(moderator => moderator.login === this.user?.login) ||
        userIsHighAuthority(this.user);
  }

  private showEditForm() {
    this.editMode = true;
  }

  private hideEditForm() {
    this.editMode = false;
  }

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
.topic
  panel()
  padding $minor
  border 1px solid
  theme(border-color, $border)

  &__meta
    display flex
    justify-content space-between
    margin $minor 0
    secondary()

  &__controls
    display none

  &:hover &__controls
    display block

  &__control
    margin-left $medium

</style>
