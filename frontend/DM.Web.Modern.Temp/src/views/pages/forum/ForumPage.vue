<script setup lang="ts">
import { useRoute } from "vue-router";
import { useForumStore, useUserStore } from "@/stores";
import { storeToRefs } from "pinia";
import router, { extractNumberParam } from "@/router";
import type { ForumId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import TheButton from "@/components/inputs/TheButton.vue";
import TheIcon from "@/components/icons/TheIcon.vue";
import { IconType } from "@/components/icons/iconType";
import { useModal } from "vue-final-modal";
import CreateTopic from "@/views/pages/forum/CreateTopic.vue";
import { computed } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";

const route = useRoute();
const { user } = storeToRefs(useUserStore());
const forumStore = useForumStore();
const { moderators, selectedForum } = storeToRefs(forumStore);
const { trySelectForum, fetchModerators, fetchTopics } = forumStore;

const canCreateTopic = computed(() => {
  if (!user.value) return false;
  if (userIsHighAuthority(user.value)) return true;
  return selectedForum.value!.id !== "Новости проекта";
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
    <the-button v-if="canCreateTopic" @click="openCreateTopic"
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
