<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute, useRouter } from "vue-router";
import { useForumStore, useUserStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { storeToRefs } from "pinia";
import type { Topic, TopicId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import { computed, ref } from "vue";
import { type DmMenuItem } from "@/components/ui-kit/DmMenu.vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import forumApi from "@/api/requests/forumApi";
import useEditMode from "@/composables/useEditMode";

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
        label: topic.value!.closed ? "Открыть" : "Закрыть",
        icon: topic.value!.closed ? IconType.Open : IconType.Closed,
        command: toggleClose,
      },
      {
        label: topic.value!.attached ? "Открепить" : "Прикрепить",
        icon: IconType.Pinned,
        command: togglePinned,
      },
      {
        label: "Удалить",
        icon: IconType.Remove,
        command: removeTopic,
      },
    );
  }
  if (user.value.login === topic.value!.author.login) {
    result.unshift({
      label: "Редактировать",
      icon: IconType.Edit,
      command: initializeEditMode,
    });
  }
  return result;
});

const { isActive, acquire, release } = useEditMode();
const loading = ref(false);

const topicToEdit = ref<Topic | null>(null);
const initializeEditMode = async () => {
  if (topicToEdit.value === null) {
    loading.value = true;
    const { data } = await forumApi.getTopicForUpdate(topic.value!.id);
    topicToEdit.value = data!.resource;
    loading.value = false;
  }
  acquire();
};
const updateTopic = async () => {
  loading.value = true;
  await forumApi.updateTopic(topic.value!.id, {
    title: topicToEdit.value!.title,
    description: topicToEdit.value!.description,
  });
  loading.value = false;
  await trySelectTopic(topic.value!.id);
};

const toggleClose = async () => {
  loading.value = true;
  await forumApi.updateTopic(topic.value!.id, {
    closed: !topic.value!.closed,
  });
  topic.value!.closed = !topic.value!.closed;
  loading.value = false;
};
const togglePinned = async () => {
  loading.value = true;
  await forumApi.updateTopic(topic.value!.id, {
    attached: !topic.value!.attached,
  });
  topic.value!.attached = !topic.value!.attached;
  loading.value = false;
};
const removeTopic = async () => {
  loading.value = true;
  await forumApi.deleteTopic(topic.value!.id);
  loading.value = false;
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
      <dm-form v-if="isActive" @submit="updateTopic">
        <dm-field>
          <dm-input
            id="edit-topic-title"
            v-model="topicToEdit!.title"
            placeholder="Название"
          />
        </dm-field>
        <dm-field>
          <dm-text
            id="edit-topic-description"
            v-model="topicToEdit!.description"
            placeholder="Описание"
          />
        </dm-field>
        <template #controls>
          <dm-button type="submit" label="Сохранить" />
          <a @click="release">Отмена</a>
        </template>
      </dm-form>
      <template v-else>
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
          :loading="loading"
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
