<script setup lang="ts">
import { IconType } from "@/components/icons/iconType";
import ThePaging from "@/components/ThePaging.vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useForumStore } from "@/stores";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import UserLink from "@/components/community/UserLink.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import HumanDate from "@/components/dates/HumanDate.vue";

const route = useRoute();
const { topics } = storeToRefs(useForumStore());
</script>

<template>
  <the-paging
    v-if="topics"
    :paging="topics.paging!"
    :to="{ name: 'forum', params: route.params }"
  />

  <div class="topics-list_header">
    <div>Тема</div>
    <div>Дата</div>
    <div>Автор</div>
    <div>
      <the-icon :font="IconType.CommentsNoUnread" />
    </div>
    <div>Последнее сообщение</div>
  </div>

  <the-loader v-if="!topics" :big="true" />
  <secondary-text v-else-if="!topics.resources.length" class="topics-list_none"
    >Еще не создано ни одной темы
  </secondary-text>
  <div
    v-for="topic in topics.resources"
    v-else
    :key="topic.id"
    :class="{
      'topics-list_row': true,
      closed: topic.closed,
      attached: topic.attached,
    }"
  >
    <router-link
      :to="{
        name: 'topic',
        params: {
          id: topic.id,
          n: topic.commentsCount - topic.unreadCommentsCount,
        },
      }"
      class="topics-list_row-title"
    >
      <the-icon v-if="topic.attached" :font="IconType.Attached" />
      <the-icon v-if="topic.closed" :font="IconType.Closed" />
      {{ topic.title }}
    </router-link>
    <div><human-date :date="topic.created!" format="DD.MM.YYYY" /></div>
    <div><user-link :user="topic.author!" /></div>
    <div>
      {{ topic.commentsCount }}
      <span class="topics-list_row-unread" v-if="topic.unreadCommentsCount"
        >(+{{ topic.unreadCommentsCount }})</span
      >
    </div>
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
@import "@/assets/styles/Grid"
$grid-template: [title] 40% [date] 12% [author] 14% [count] auto [lastComment] 26%

.topics-list_header
  +grid-head($grid-template)

.topics-list_row
  +grid($grid-template)
  &:hover
    +theme(background-color, $panel-background-hover)
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
  margin: $medium 0
  text-align: center
</style>
