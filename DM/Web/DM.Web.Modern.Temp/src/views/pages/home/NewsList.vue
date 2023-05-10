<template>
  <block-title>Последние новости</block-title>
  <the-loader v-if="!store.news" />
  <template v-else-if="!store.news.length">Ничего нового</template>
  <template v-else>
    <div v-for="article in store.news" :key="article.id" class="article">
      <router-link
        class="article-title"
        :to="{ name: 'topic', params: { id: article.id } }"
      >
        {{ article.title }}
      </router-link>
      <div class="article-description" v-html="article.description"></div>
      <secondary-text>
        <user-link :user="article.author!" />
        <human-timespan :date="article.created!" />&nbsp;<the-icon
          :font="IconType.CommentsNoUnread"
        />
      </secondary-text>
    </div>
  </template>
</template>

<script setup lang="ts">
import BlockTitle from "@/components/layout/BlockTitle.vue";
import TheLoader from "@/components/TheLoader.vue";
import UserLink from "@/components/community/UserLink.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import { IconType } from "@/components/icons/iconType";
import { useForumStore } from "@/stores";
import { onMounted } from "vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";

const store = useForumStore();

onMounted(() => store.fetchNews());
</script>
