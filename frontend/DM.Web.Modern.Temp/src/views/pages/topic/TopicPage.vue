<script setup lang="ts">
import { IconType } from "@/components/icons/iconType";
import { useRoute } from "vue-router";
import { useForumStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { storeToRefs } from "pinia";
import TheComment from "@/components/comments/TheComment.vue";
import type { TopicId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";

const route = useRoute();
const forumStore = useForumStore();
const { trySelectTopic, fetchComments } = forumStore;
const { selectedTopic: topic } = storeToRefs(forumStore);

async function fetchData() {
  await trySelectTopic(route.params.id as TopicId);
  await fetchComments(extractNumberParam(route.params.n));
}

useFetchData(
  () => fetchData(),
  [
    {
      param: (p) => p.id,
      callback: () => fetchData(),
    },
    {
      param: (p) => p.n,
      callback: (n) => fetchComments(extractNumberParam(n)),
    },
  ],
);
</script>

<template>
  <template v-if="topic">
    <div class="topic-header">
      <page-title>{{ topic.title }}</page-title>
      <router-link :to="{ name: 'forum', params: { id: topic.forum.id } }">
        <the-icon :font="IconType.ArrowLeft" />
        Назад на форум "{{ topic.forum.id }}"
      </router-link>
    </div>
    <the-comment
      :author="topic.author!"
      :created="topic.created!"
      :comment="topic.description!"
    />
  </template>
  <the-loader v-else :big="true" />
  <router-view />
</template>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

.topic-header
  display: flex
  justify-content: space-between
  align-items: baseline

.topic-description
  margin-bottom: $medium
</style>
