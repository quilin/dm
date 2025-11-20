<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute } from "vue-router";
import { useForumStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { storeToRefs } from "pinia";
import type { TopicId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";

const route = useRoute();
const forumStore = useForumStore();
const { trySelectTopic, fetchComments } = forumStore;
const { selectedTopic: topic } = storeToRefs(forumStore);

async function fetchData() {
  await trySelectTopic(route.params.id as TopicId);
  await fetchComments(extractNumberParam(route.params.n));
}

useFetchData(fetchData, [
  {
    param: (p) => p.id,
    callback: fetchData,
  },
  {
    param: (p) => p.n,
    callback: (n) => fetchComments(extractNumberParam(n)),
  },
]);
</script>

<template>
  <template v-if="topic">
    <div class="topic-header">
      <page-title>{{ topic.title }}</page-title>
      <router-link :to="{ name: 'forum', params: { id: topic.forum.id } }">
        <dm-icon :font="IconType.ArrowLeft" />
        Назад на форум "{{ topic.forum.id }}"
      </router-link>
    </div>
    <div
      v-if="topic.description"
      class="topic-description"
      v-html="topic.description"
    />
    <user-link :user="topic.author" />,
    <secondary-text>
      <human-timespan date="topic.created" />
    </secondary-text>
  </template>
  <dm-loader v-else :big="true" />
  <router-view />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.topic-header
  display: flex
  justify-content: space-between
  align-items: baseline

.topic-description
  margin-bottom: Variables.$minor
</style>
