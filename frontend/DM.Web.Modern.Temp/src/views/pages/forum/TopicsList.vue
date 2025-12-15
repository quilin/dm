<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute } from "vue-router";
import { useForumStore } from "@/stores";
import ForumTopic from "@/views/pages/forum/ForumTopic.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import DmPaging from "@/components/ui-kit/DmPaging.vue";
import messages from "@/views/pages/forum/TopicsList.i18n";
import { useI18n } from "vue-i18n";

const route = useRoute();
const store = useForumStore();
const { t } = useI18n({ messages });
</script>

<template>
  <dm-paging
    v-if="store.topics !== null"
    :paging="store.topics.paging!"
    :to="{ name: 'forum', params: route.params }"
  />

  <div class="topics_list-header">
    <div>{{ t("title") }}</div>
    <div>{{ t("date") }}</div>
    <div>{{ t("author") }}</div>
    <div class="topics_list-counter">
      <dm-icon :font="IconType.CommentsNoUnread" />
    </div>
    <div class="topics_list-counter">
      <dm-icon :font="IconType.Like" />
    </div>
    <div>{{ t("latestComment") }}</div>
  </div>

  <template v-if="store.attachedTopics !== null">
    <forum-topic
      v-for="topic in store.attachedTopics"
      :topic="topic"
      :key="topic.id"
    />
  </template>
  <dm-loader
    v-if="store.topics === null || store.attachedTopics === null"
    :big="true"
  />
  <secondary-text
    v-else-if="
      store.topics.resources.length + store.attachedTopics.length === 0
    "
    class="topics_list-none"
    >{{ t("empty") }}</secondary-text
  >
  <forum-topic
    v-else
    v-for="topic in store.topics.resources"
    :topic="topic"
    :key="topic.id"
  />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"
@use "@/assets/styles/Grid"
$grid-template: [title] 35% [date] 12% [author] 14% [count] 1fr [likes] 1fr [lastComment] 26%

.topics_list-header
  +Grid.grid-head($grid-template)

.topics_list-counter
  text-align: center

.topics_list-row
  +Grid.grid($grid-template)
  &:hover
    +Themes.theme(background-color, Themes.$panel-background-hover)
  &.closed
    opacity: 0.7
    &.attached
      opacity: initial

.topics_list-row_title
  display: block
  position: relative
  & .attached
    font-weight: bold

.topics_list-row_unread
  font-weight: bold

.topics_list-none
  display: block
  margin: Variables.$medium 0
  text-align: center
</style>
