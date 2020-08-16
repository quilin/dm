<template>
  <div>
    <div class="content-title">Комментарии</div>
    <loader v-if="loading" />
    <topic-comment
        v-else-if="comments && comments.length"
        v-for="comment in comments"
        :key="comment.id"
        :comment="comment"
        @deleted="fetchData"
    />
    <div v-else class="topicComments__empty">Здесь еще никто ничего не написал</div>
    <paging
        class="topicComments__paging"
        v-if="paging"
        :paging="paging"
        :to="{name: 'topic', params: $route.params}"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Comment } from '@/api/models/forum';
import { Paging } from '@/api/models/common';
import TopicComment from './TopicComment.vue';

@Component({
  components: { TopicComment },
})
export default class TopicComments extends Vue {
  @Getter('forum/comments')
  private comments!: Comment[];

  @Action('forum/fetchComments')
  private fetchComments: any;

  @Getter('forum/commentsPaging')
  private paging!: Paging | null;

  private loading = false;

  @Watch('paging.current')
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
.topicComments
  &__paging
    margin-top $medium
</style>
