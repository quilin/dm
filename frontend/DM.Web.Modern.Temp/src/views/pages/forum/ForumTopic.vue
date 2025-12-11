<script setup lang="ts">
import type { Topic } from "@/api/models/forum";
import { IconType } from "@/components/ui-kit/iconType";
import { computed } from "vue";

const props = defineProps<{ topic: Topic }>();
const topicDescription = computed(() => {
  return props.topic.description.length <= 100
    ? props.topic.description
    : `${props.topic.description.substring(0, 100)}...`;
});
</script>

<template>
  <div
    :key="topic.id"
    :class="{
      'topics_list-row': true,
      closed: topic.closed,
      attached: topic.attached,
    }"
  >
    <router-link
      :to="{
        name: 'topic',
        params: {
          id: topic.id,
          n:
            topic.commentsCount && topic.unreadCommentsCount
              ? topic.commentsCount - topic.unreadCommentsCount + 1
              : topic.commentsCount || undefined,
        },
      }"
      class="topics_list-row_title"
    >
      <dm-icon v-if="topic.attached" :font="IconType.Pinned" />
      <dm-icon v-if="topic.closed" :font="IconType.Closed" />
      {{ topic.title }}<br />
      <secondary-text v-if="topicDescription"
        ><span v-html="topicDescription"
      /></secondary-text>
    </router-link>
    <div><human-date :date="topic.created!" format="DD.MM.YYYY" /></div>
    <div><user-link :user="topic.author!" /></div>
    <div class="topics_list-counter">
      {{ topic.commentsCount }}
      <span class="topics_list-row_unread" v-if="topic.unreadCommentsCount"
        >(+{{ topic.unreadCommentsCount }})</span
      >
    </div>
    <div class="topics_list-counter">{{ topic.likes.length }}</div>
    <div>
      <template v-if="topic.lastComment">
        <user-link :user="topic.lastComment.author" />,
        <router-link
          :to="{
            name: 'topic',
            params: { id: topic.id, n: topic.commentsCount },
          }"
        >
          <human-timespan :date="topic.lastComment.created" />
        </router-link>
      </template>
    </div>
  </div>
</template>

<style scoped lang="sass">
.topics_list-counter
  text-align: center
</style>
