<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useForumStore } from "@/stores";
import { onMounted } from "vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import UserLink from "@/components/community/UserLink.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import messages from "@/views/pages/home/NewsList.i18n";
import { useI18n } from "vue-i18n";

const store = useForumStore();
const { t } = useI18n({ messages });

onMounted(() => store.fetchNews());
</script>

<template>
  <block-title>{{ t("title") }}</block-title>
  <dm-loader v-if="store.news === null" />
  <secondary-text v-else-if="!store.news.length">{{
    t("empty")
  }}</secondary-text>

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
