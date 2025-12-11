<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useForumStore } from "@/stores";
import ForumTopic from "@/views/pages/forum/ForumTopic.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";

const route = useRoute();
const { topics, attachedTopics } = storeToRefs(useForumStore());
</script>

<template>
  <dm-paging
    v-if="topics"
    :paging="topics.paging!"
    :to="{ name: 'forum', params: route.params }"
  />

  <div class="topics_list-header">
    <div>Тема</div>
    <div>Дата</div>
    <div>Автор</div>
    <div class="topics_list-counter">
      <dm-icon :font="IconType.CommentsNoUnread" />
    </div>
    <div class="topics_list-counter">
      <dm-icon :font="IconType.Like" />
    </div>
    <div>Последнее сообщение</div>
  </div>

  <template v-if="attachedTopics !== null">
    <forum-topic
      v-for="topic in attachedTopics"
      :topic="topic"
      :key="topic.id"
    />
  </template>
  <dm-loader v-if="!topics || attachedTopics === null" :big="true" />
  <secondary-text
    v-else-if="topics.resources.length + attachedTopics.length === 0"
    class="topics_list-none"
    >Еще не создано ни одной темы</secondary-text
  >
  <forum-topic
    v-else
    v-for="topic in topics.resources"
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
