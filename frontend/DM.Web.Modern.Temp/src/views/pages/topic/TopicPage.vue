<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute, useRouter } from "vue-router";
import { useForumStore, useUserStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { storeToRefs } from "pinia";
import type { TopicId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import { computed, ref } from "vue";
import DmMenu, { type DmMenuItem } from "@/components/ui-kit/DmMenu.vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import forumApi from "@/api/requests/forumApi";
import EditTopic from "@/views/pages/forum/EditTopic.vue";

const route = useRoute();
const router = useRouter();

const forumStore = useForumStore();
const { trySelectTopic, fetchComments } = forumStore;
const { selectedTopic: topic } = storeToRefs(forumStore);
const { user } = storeToRefs(useUserStore());

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

const topicActions = computed(() => {
  const result: DmMenuItem[] = [];
  if (!user.value) return result;

  if (userIsHighAuthority(user.value)) {
    result.push(
      {
        label: "Удалить",
        icon: IconType.Remove,
        command: removeTopic,
      },
      {
        label: topic.value!.closed ? "Открыть" : "Закрыть",
        icon: topic.value!.closed ? IconType.Open : IconType.Closed,
        command: toggleClose,
      },
      {
        label: topic.value!.attached ? "Открепить" : "Прикрепить",
        icon: IconType.Pinned,
        command: togglePinned,
      },
    );
  }
  if (user.value.login === topic.value!.author.login) {
    result.unshift({
      label: "Редактировать",
      icon: IconType.Edit,
      command: () => (editMode.value = true),
    });
  }
  return result;
});

const editMode = ref(false);
const toggleClose = async () => {
  await forumApi.updateTopic(topic.value!.id, {
    closed: !topic.value!.closed,
  });
  topic.value!.closed = !topic.value!.closed;
};
const togglePinned = async () => {
  await forumApi.updateTopic(topic.value!.id, {
    attached: !topic.value!.attached,
  });
  topic.value!.attached = !topic.value!.attached;
};
const removeTopic = async () => {
  await forumApi.deleteTopic(topic.value!.id);
  await router.push({
    name: "forum",
    params: { n: undefined, id: topic.value!.forum.id },
  });
};
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
    <div class="topic-content">
      <edit-topic
        :topic="topic"
        v-model:active="editMode"
        @updated="trySelectTopic(route.params.id as TopicId)"
      />
      <template v-if="!editMode">
        <div
          v-if="topic.description"
          class="topic-description"
          v-html="topic.description"
        />
        <user-link :user="topic.author" />,
        <secondary-text
          ><human-timespan :date="topic.created"
        /></secondary-text>
        <dm-menu
          v-if="topicActions"
          class="topic-actions"
          :items="topicActions"
        />
      </template>
    </div>
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

.topic-content
  position: relative

.topic-actions
  position: absolute
  bottom: 0
  right: 0

.topic-description
  margin-bottom: Variables.$minor
</style>
