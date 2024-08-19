<script setup lang="ts">
import { IconType } from "@/components/icons/iconType";
import { useForumStore } from "@/stores";
import { onMounted } from "vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import { storeToRefs } from "pinia";

const store = useForumStore();
const { news } = storeToRefs(store);

onMounted(() => store.fetchNews());
</script>

<template>
  <block-title>Последние новости</block-title>
  <the-loader v-if="!news" />
  <secondary-text v-else-if="!news.length">Ничего нового</secondary-text>

  <div v-else v-for="article in news" :key="article.id" class="article">
    <router-link
      class="article-title"
      :to="{ name: 'topic', params: { id: article.id } }"
    >
      {{ article.title }}
    </router-link>
    <div class="article-description" v-html="article.description"></div>
    <div>
      <user-link :user="article.author!" />
      <human-timespan :date="article.created!" />&nbsp;<the-icon
        :font="IconType.CommentsNoUnread"
      />
    </div>
  </div>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

.article
  margin: $medium 0

.article-title
  font-weight: bold

.article-description
  margin: $small 0
</style>
