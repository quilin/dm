<template>
  <div>
    <div class="content-title">Комментарии</div>
    <loader v-if="loading" />
    <topic-comment
        v-else-if="comments && comments.resources.length"
        v-for="comment in comments.resources"
        :key="comment.id"
        :comment="comment"
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

@Component({
  components: { TopicComment },
})
export default class TopicComments extends Vue {
  private loading = false;

  @Getter('forum/comments')
  private comments!: ListEnvelope<Comment>;

  @Action('forum/fetchComments')
  private fetchComments: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    const { id, n } = this.$route.params;

    this.loading = true;
    await this.fetchComments({ id, n });
    this.loading = false;
  }
}
</script>

<style scoped lang="stylus">
.topic-сomments
  &__paging
    margin-top $medium
</style>
