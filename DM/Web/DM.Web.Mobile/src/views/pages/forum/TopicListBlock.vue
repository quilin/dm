<template>
  <div class="article">
    <div class="article-title">
      <router-link :to="{name: 'Topic', params: {id: article.id}}">
        {{article.title}}
      </router-link>
    </div>
    <div class="article-description" v-html="article.description"></div>
    <div class="article-data">
      <router-link :to="{name: 'User', params: {login: article.author.login}}">{{article.author.login}}</router-link>,
      <humanTimespan :time="article.created"/>
      <icon v-if="article.unreadCommentsCount" :font="IconType.CommentsUnread"/>
      <icon v-else :font="IconType.CommentsNoUnread"/>
    </div>
  </div>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from 'vue-property-decorator';
  import IconType from '@/components/iconType';


  @Component
  export default class TopicListBlock extends Vue {
    private IconType: typeof IconType = IconType;

    @Prop()
    private article!: any;

  }
</script>


<style scoped lang="stylus">
  .article
    margin $small 0

    .article-title
      minorTitle()

    .article-description
      margin-bottom $minor

    .article-data
      secondary()

</style>
