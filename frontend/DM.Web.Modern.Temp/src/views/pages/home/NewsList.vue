<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useForumStore } from "@/stores";
import { onMounted } from "vue";

const store = useForumStore();

onMounted(() => store.fetchNews());
</script>

<template>
  <block-title>Последние новости</block-title>
  <dm-loader v-if="store.news === null" />
  <secondary-text v-else-if="!store.news.length">Ничего нового</secondary-text>

  <div v-else v-for="article in store.news" :key="article.id" class="article">
    <router-link
      class="article-title"
      :to="{ name: 'topic', params: { id: article.id } }"
    >
      {{ article.title }}
    </router-link>
    <div class="article-description" v-html="article.description"></div>
    <div>
      <user-link :user="article.author!" />,
      <secondary-text>
        <human-timespan :date="article.created" /> </secondary-text
      >&nbsp;<router-link
        :to="{
          name: 'topic',
          params: {
            id: article.id,
            n:
              article.commentsCount && article.unreadCommentsCount
                ? article.commentsCount - article.unreadCommentsCount + 1
                : article.commentsCount || undefined,
          },
        }"
      >
        <dm-icon
          :font="
            article.unreadCommentsCount > 0
              ? IconType.CommentsUnread
              : IconType.CommentsNoUnread
          "
        />&nbsp;<template v-if="article.unreadCommentsCount">{{
          article.unreadCommentsCount
        }}</template>
      </router-link>
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.article
  margin: Variables.$medium 0

.article-title
  font-weight: bold

.article-description
  margin: Variables.$small 0
</style>
