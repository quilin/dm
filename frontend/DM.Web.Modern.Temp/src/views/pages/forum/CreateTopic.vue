<script setup lang="ts">
import { ref } from "vue";
import { useForumStore } from "@/stores";

import type { Topic } from "@/api/models/forum";
import type { Post } from "@/api/models";

const forumStore = useForumStore();

const topic = ref<Post<Topic>>({
  title: "",
  description: "",
  attached: false,
  closed: false,
  forum: forumStore.selectedForum!,
});

const emit = defineEmits<{
  (e: "created", topic: Topic): void;
  (e: "cancelled"): void;
}>();

const loading = ref(false);
const tryCreateTopic = async () => {
  loading.value = true;
  const createdTopic = await forumStore.createTopic(topic.value);
  loading.value = false;
  emit("created", createdTopic);
};
</script>

<template>
  <dm-dialog :with-form="true">
    <page-title>Создание темы</page-title>
    <dm-form @submit="tryCreateTopic">
      <dm-field>
        <dm-input
          id="topic-title"
          v-model="topic.title"
          placeholder="Название темы"
        />
      </dm-field>
      <dm-field>
        <dm-text
          id="topic-description"
          v-model="topic.description"
          placeholder="Описание"
        />
      </dm-field>
      <template #controls>
        <dm-button label="Создать" :loading="loading" type="submit" />
        <a @click="$emit('cancelled')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="sass"></style>
