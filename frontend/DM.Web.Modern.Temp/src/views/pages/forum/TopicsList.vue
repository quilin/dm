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

  <div class="topics-list_header">
    <div>Тема</div>
    <div>Дата</div>
    <div>Автор</div>
    <div>
      <dm-icon :font="IconType.CommentsNoUnread" />
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
    class="topics-list_none"
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
$grid-template: [title] 40% [date] 12% [author] 14% [count] auto [lastComment] 26%

.topics-list_header
  +Grid.grid-head($grid-template)

.topics-list_row
  +Grid.grid($grid-template)
  &:hover
    +Themes.theme(background-color, Themes.$panel-background-hover)
  &.closed
    opacity: 0.7
    &.attached
      opacity: initial

.topics-list_row-title
  display: block
  position: relative
  & .attached
    font-weight: bold

.topics-list_row-unread
  font-weight: bold

.topics-list_none
  display: block
  margin: Variables.$medium 0
  text-align: center
</style>
