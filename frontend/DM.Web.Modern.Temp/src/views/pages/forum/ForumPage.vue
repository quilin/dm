<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import CreateTopic from "@/views/pages/forum/CreateTopic.vue";

import { computed } from "vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useModal } from "vue-final-modal";

import { useForumStore, useUserStore } from "@/stores";
import router, { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";

import { userIsHighAuthority } from "@/api/models/community/helpers";
import type { ForumId } from "@/api/models/forum";

const route = useRoute();
const { user } = storeToRefs(useUserStore());
const forumStore = useForumStore();
const { moderators, selectedForum } = storeToRefs(forumStore);
const { trySelectForum, fetchModerators, fetchTopics, markAllTopicsAsRead } =
  forumStore;

const canCreateTopic = computed(() => {
  if (!user.value) return false;
  if (userIsHighAuthority(user.value)) return true;
  return selectedForum.value?.id !== "Новости проекта";
});

async function fetchData() {
  const forumId = route.params.id as ForumId;
  await trySelectForum(forumId);

  await Promise.all([
    fetchModerators(),
    fetchTopics(extractNumberParam(route.params.n)),
  ]);
}

useFetchData(fetchData, [
  {
    param: (p) => p.id,
    callback: fetchData,
  },
  {
    param: (p) => p.n,
    callback: (n) => fetchTopics(extractNumberParam(n)),
  },
]);

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
  <div class="forum-header">
    <page-title>Форум | {{ route.params.id }}</page-title>
    <a @click="markAllTopicsAsRead">Отметить все темы прочитанными</a>
  </div>

  <div class="forum-info">
    <div class="forum-info_moderators">
      <block-title class="forum-info_moderators-title">Модераторы:</block-title>
      <dm-loader v-if="!moderators" class="forum-info_moderators-loader" />
      <user-link
        v-else
        v-for="user in moderators"
        :key="user.login"
        :user="user"
      />
    </div>
    <dm-button
      label="Новая тема"
      :icon="IconType.Add"
      v-if="canCreateTopic"
      @click="openCreateTopic"
    />
  </div>

  <router-view />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.forum-header
  display: flex
  justify-content: space-between
  align-items: baseline

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
