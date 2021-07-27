<template>
  <div>
    <div class="content-title">Последние новости</div>
    <loader v-if="!news" />
    <div v-else-if="!news.length">
      Ничего нового!
    </div>
    <template v-else>
      <div v-for="article in news" :key="article.id">
        <div class="article">
          <div class="article-title">
            <router-link :to="{name: 'topic', params: {id: article.id}}">
              {{article.title}}
            </router-link>
          </div>
          <div class="article-description" v-html="article.description"></div>
          <div class="article-data">
            <user-link :user="article.author" />
            <human-timespan :date="article.created" />&#32;
            <icon v-if="!article.unreadCommentsCount" :font="IconType.CommentsNoUnread" />
          </div>
        </div>
      </div>
      <router-link :to="{name: 'forum', params: { id: 'Новости проекта'} }" class="news-rest">
        К остальным новостям <icon :font="IconType.Forward" />
      </router-link>
    </template>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import { Topic } from '@/api/models/forum';
import IconType from '@/components/iconType';

@Component({})
export default class News extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('forum/news')
  private news!: Topic[];

  @Action('forum/fetchNews')
  private fetchNews: any;

  private mounted(): void {
    this.fetchNews();
  }
}
</script>

<style scoped lang="stylus">
.news-rest
  font-weight bold

.article
  padding $small 0

.article-title
  margin-bottom $small
  font-weight bold

.article-data
  margin-top $minor
  secondary()
</style>
