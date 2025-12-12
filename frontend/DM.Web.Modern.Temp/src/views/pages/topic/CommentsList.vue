<script setup lang="ts">
import { useForumStore, useUserStore } from "@/stores";
import { useRoute, useRouter } from "vue-router";
import TopicComment from "@/views/pages/topic/TopicComment.vue";
import CreateComment from "@/views/pages/topic/CreateComment.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import DmPaging from "@/components/ui-kit/DmPaging.vue";

const route = useRoute();
const router = useRouter();
const forumStore = useForumStore();
const userStore = useUserStore();

const redirectToLastPage = () => {
  router.push({
    name: "topic",
    params: {
      id: forumStore.selectedTopic!.id,
      n: forumStore.comments!.paging!.total + 1,
    },
  });
};
</script>

<template>
  <dm-paging
    v-if="forumStore.comments"
    :paging="forumStore.comments.paging!"
    :to="{ name: 'topic', params: route.params }"
  />
  <dm-loader v-if="forumStore.comments === null" :big="true" />
  <secondary-text
    v-else-if="forumStore.comments.resources.length === 0"
    class="comments-none"
    >Комментариев пока нет...</secondary-text
  >
  <topic-comment
    v-else
    v-for="comment in forumStore.comments.resources"
    :key="comment.id"
    :comment="comment"
  />

  <create-comment v-if="userStore.user" @created="redirectToLastPage" />
</template>

<style scoped lang="sass">
.comments-none
  display: block
  text-align: center
</style>
