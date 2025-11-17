<script setup lang="ts">
import LightboxTitle from "@/components/layout/LightboxTitle.vue";
import TheLightbox from "@/components/layout/TheLightbox.vue";
import TheForm from "@/components/inputs/form/TheForm.vue";
import TextArea from "@/components/inputs/TextArea.vue";
import { reactive, ref } from "vue";
import SimpleButton from "@/components/inputs/SimpleButton.vue";
import type { Topic } from "@/api/models/forum";
import type { Post } from "@/api/models";
import { useForumStore } from "@/stores";

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
    <the-form @submit="tryCreateTopic" :valid="true" :loading="loading">
      <input v-model="topic.title" type="text" />
      <text-area v-model="topic.description" />
      <template #controls>
        <simple-button :loading="loading" type="submit">Создать</simple-button>
        <a @click="$emit('cancelled')">Отмена</a>
      </template>
    </the-form>
  </the-lightbox>
</template>

<style scoped lang="sass"></style>
