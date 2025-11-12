<script setup lang="ts">
import { useRoute } from "vue-router";
import { useForumStore } from "@/stores";
import { storeToRefs } from "pinia";
import router, { extractNumberParam } from "@/router";
import type { ForumId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import TheButton from "@/components/inputs/TheButton.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import { IconType } from "@/components/icons/iconType";
import { useModal } from "vue-final-modal";
import CreateTopic from "@/views/pages/forum/CreateTopic.vue";

const route = useRoute();
const forumStore = useForumStore();
const { moderators } = storeToRefs(forumStore);
const { trySelectForum, fetchModerators, fetchTopics } = forumStore;

async function fetchData() {
  const forumId = route.params.id as ForumId;
  await trySelectForum(forumId);

  await Promise.all([
    fetchModerators(),
    fetchTopics(extractNumberParam(route.params.n)),
  ]);
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
      callback: () => fetchTopics(extractNumberParam(route.params.n)),
    },
  ],
);

const { open: openCreateTopic, close: closeCreateTopic } = useModal({
  component: CreateTopic,
  attrs: {
    onCancelled: () => closeCreateTopic(),
    onCreated: (topic) => {
      closeCreateTopic();
      router.push({ name: "topic", params: { id: topic.id } });
    },
  },
});
</script>

<template>
  <page-title>Форум | {{ route.params.id }}</page-title>

  <div class="forum-info">
    <div class="forum-info_moderators">
      <block-title class="forum-info_moderators-title">Модераторы:</block-title>
      <the-loader v-if="!moderators" class="forum-info_moderators-loader" />
      <user-link
        v-else
        v-for="user in moderators"
        :key="user.login"
        :user="user"
      />
    </div>
    <the-button @click="openCreateTopic"
      ><the-icon :font="IconType.Add" /> Новая тема</the-button
    >
  </div>

  <router-view />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.forum-info
  display: flex
  justify-content: space-between
  align-items: baseline
  margin: Variables.$medium 0

.forum-info_moderators-title
  display: inline-block
  margin: 0 Variables.$medium 0 0

.forum-info_moderators-loader
  display: inline-block
</style>
