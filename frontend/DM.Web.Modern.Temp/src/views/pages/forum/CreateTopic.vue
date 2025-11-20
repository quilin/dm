<script setup lang="ts">
import LightboxTitle from "@/components/layout/LightboxTitle.vue";
import TheLightbox from "@/components/layout/TheLightbox.vue";

import { reactive, ref } from "vue";
import { useForumStore } from "@/stores";

import type { Topic } from "@/api/models/forum";
import type { Post } from "@/api/models";

const { createTopic, selectedForum } = useForumStore();

const topic = reactive<Post<Topic>>({
  title: "",
  description: "",
  attached: false,
  closed: false,
  forum: selectedForum!,
});

const emit = defineEmits<{
  (e: "created", topic: Topic): void;
  (e: "cancelled"): void;
}>();

const loading = ref(false);
const tryCreateTopic = async () => {
  loading.value = true;
  alert(selectedForum?.id);
  const createdTopic = await createTopic(topic);
  loading.value = false;
  emit("created", createdTopic);
};
</script>

<template>
  <the-lightbox>
    <lightbox-title>Создание темы</lightbox-title>
    <dm-form :model="topic" @submit="tryCreateTopic">
      <dm-field>
        <dm-input
          id="title"
          v-model="topic.title"
          placeholder="Название темы"
        />
      </dm-field>
      <dm-field>
        <dm-text
          id="description"
          v-model="topic.description"
          placeholder="Описание"
        />
      </dm-field>
      <template #controls>
        <dm-button label="Создать" :loading="loading" type="submit" />
        <a @click="$emit('cancelled')">Отмена</a>
      </template>
    </dm-form>
  </the-lightbox>
</template>

<style scoped lang="sass"></style>
