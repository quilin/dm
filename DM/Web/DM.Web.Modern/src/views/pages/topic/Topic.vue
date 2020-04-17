<template>
  <div>
    <div v-if="topic">
      <div class="page-title">{{topic.title}}</div>
      <div class="description" v-html="topic.description"></div>
      <div class="data">
        <router-link :to="{name: 'user', params: {login: topic.author.login}}">{{topic.author.login}}</router-link>,
        <human-timespan :date="topic.created" />
      </div>
      <router-link :to="{name: 'forum', params: {id: topic.forum.id}}">
        <icon :font="IconType.ArrowLeft" /> Назад на форум "{{topic.forum.id}}"
      </router-link>
    </div>
    <loader v-else class="topic-loader" />
    <div class="content-title">Комментарии</div>
    <loader v-if="!comments" />
    <template v-else-if="comments.length">
      <div v-for="comment in comments" :key="comment.id" class="comment">
        <div v-html="comment.text"></div>
        <div class="comment-data">
          <router-link :to="{name: 'user', params: {login: comment.author.login}}">{{comment.author.login}}</router-link>,
          <human-timespan :date="comment.created" />
          <template v-if="comment.updated">(изменен <human-timespan :date="comment.updated" />)</template>
        </div>
      </div>
    </template>
    <div v-else class="comments-nothing">Здесь еще никто ничего не написал</div>
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Topic, Forum, Comment } from '@/api/models/forum';
import IconType from '@/components/iconType';

const namespace = 'forum';

@Component({})
export default class TopicPage extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('topic', { namespace })
  private topic!: Topic;

  @Getter('comments', { namespace })
  private comments!: Comment[];

  @Action('selectTopic', { namespace })
  private selectTopic: any;

  @Action('fetchComments', { namespace })
  private fetchComments: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const id = this.$route.params.id;
    this.selectTopic({id});
    this.fetchComments({ id, n: this.$route.params.n });
  }
}
</script>

<style scoped lang="stylus">
.data
  margin $small 0
  theme(color, $secondaryText)

.topic-loader
  margin-top $medium + $small

.comment
  block()

.comment-data
  margin-top $minor
  secondary()
</style>
