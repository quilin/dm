<script setup lang="ts">
import { useForumStore, useUserStore } from "@/stores";
import { storeToRefs } from "pinia";
import { useRoute, useRouter } from "vue-router";
import TopicComment from "@/views/pages/topic/TopicComment.vue";
import CreateComment from "@/views/pages/topic/CreateComment.vue";

const route = useRoute();
const router = useRouter();
const { comments, selectedTopic } = storeToRefs(useForumStore());
const { user } = storeToRefs(useUserStore());

const redirectToLastPage = () => {
  router.push({
    name: "topic",
    params: {
      id: selectedTopic.value!.id,
      n: comments.value!.paging!.total + 1,
    },
  });
};
</script>

<template>
  <dm-paging
    v-if="comments"
    :paging="comments.paging!"
    :to="{ name: 'topic', params: route.params }"
  />
  <dm-loader v-if="!comments" :big="true" />
  <secondary-text v-else-if="!comments.resources.length" class="comments-none"
    >Комментариев пока нет...</secondary-text
  >
  <topic-comment
    v-else
    v-for="comment in comments.resources"
    :key="comment.id"
    :comment="comment"
  />

  <create-comment
    v-if="user && !selectedTopic?.closed"
    @created="redirectToLastPage"
  />
</template>

<style scoped lang="sass">
.comments-none
  display: block
  text-align: center
</style>
