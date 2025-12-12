<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import CreateTopic from "@/views/pages/forum/CreateTopic.vue";

import { computed } from "vue";
import { useRoute } from "vue-router";
import { useModal } from "vue-final-modal";

import { useForumStore, useUserStore } from "@/stores";
import router, { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";

import { userIsHighAuthority } from "@/api/models/community/helpers";
import type { ForumId } from "@/api/models/forum";

const route = useRoute();
const userStore = useUserStore();
const forumStore = useForumStore();

const canCreateTopic = computed(() => {
  if (userStore.user === null) return false;
  if (userIsHighAuthority(userStore.user)) return true;
  return forumStore.selectedForum?.id !== "Новости проекта";
});

async function fetchData() {
  const forumId = route.params.id as ForumId;
  await forumStore.trySelectForum(forumId);

  await Promise.all([
    forumStore.fetchModerators(),
    forumStore.fetchTopics(extractNumberParam(route.params.n)),
  ]);
}

useFetchData(fetchData, [
  {
    param: (p) => p.id,
    callback: fetchData,
  },
  {
    param: (p) => p.n,
    callback: (n) => forumStore.fetchTopics(extractNumberParam(n)),
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
    <a @click="forumStore.markAllTopicsAsRead"
      >Отметить все темы прочитанными</a
    >
  </div>

  <div class="forum-info">
    <div class="forum-info_moderators">
      <block-title class="forum-info_moderators-title">Модераторы:</block-title>
      <dm-loader
        v-if="!forumStore.moderators"
        class="forum-info_moderators-loader"
      />
      <user-link
        v-else
        v-for="user in forumStore.moderators"
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
