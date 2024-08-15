<script setup lang="ts">
import { useRoute } from "vue-router";
import { useForumStore } from "@/stores";
import { storeToRefs } from "pinia";
import { onMounted, watch } from "vue";
import { extractNumberParam } from "@/router";

const route = useRoute();
const forumStore = useForumStore();
const { moderators } = storeToRefs(forumStore);
const { trySelectForum, fetchModerators, fetchTopics } = forumStore;

async function fetchData() {
  const forumId = route.params.id as string;
  await trySelectForum(forumId);

  await Promise.all([
    fetchModerators(),
    fetchTopics(extractNumberParam(route.params.n)),
  ]);
}

onMounted(() => fetchData());
watch(
  () => route.params,
  (value, oldValue) => {
    if (value.id !== oldValue.id) {
      fetchData();
    } else if (value.n !== oldValue.n) {
      fetchTopics(extractNumberParam(value.n));
    }
  }
);
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
  </div>

  <router-view />
</template>

<style scoped lang="sass">
.forum-info
  margin: $medium 0

.forum-info_moderators-title
  display: inline-block
  margin: 0 $medium 0 0

.forum-info_moderators-loader
  display: inline-block
</style>
