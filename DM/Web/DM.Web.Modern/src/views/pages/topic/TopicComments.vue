<template>
  <div>
    <div class="content-title">Комментарии</div>
    <loader v-if="loading" />
    <topic-comment
        v-else-if="comments && comments.resources.length"
        v-for="comment in comments.resources"
        :key="comment.id"
        :comment="comment"
        :editable="commentEditable(comment.author)"
        @deleted="fetchData"
    />
    <div v-else>Здесь еще никто ничего не написал</div>
    <paging
        class="topic-сomments__paging"
        v-if="comments && comments.paging"
        :paging="comments.paging"
        :to="{name: 'topic', params: $route.params}"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Comment } from '@/api/models/forum';
import { ListEnvelope } from '@/api/models/common';
import TopicComment from './TopicComment.vue';
import { User } from '@/api/models/community';
import { userIsHighAuthority } from '@/api/models/community/helpers';

@Component({
  components: { TopicComment },
})
export default class TopicComments extends Vue {
  private loading = false;

  @Getter('forum/comments')
  private comments!: ListEnvelope<Comment>;

  @Getter('user')
  private user!: User | null;

  @Getter('forum/moderators')
  private moderators!: User[];

  @Action('forum/fetchComments')
  private fetchComments: any;

  private commentEditable(author: User) {
    return author.login === this.user?.login ||
        this.moderators.some(moderator => moderator.login === this.user?.login) ||
        userIsHighAuthority(this.user);
  }

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    const id = this.$route.params.id;

    this.loading = true;

    await this.fetchComments({ id, n: this.$route.params.n });

    this.loading = false;
  }
}
</script>

<style scoped lang="stylus">
.topic-сomments
  &__paging
    margin-top $medium
</style>
