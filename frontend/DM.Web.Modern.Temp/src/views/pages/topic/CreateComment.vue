<script setup lang="ts">
import { ref } from "vue";
import { useForumStore } from "@/stores";

const store = useForumStore();

const emit = defineEmits(["created"]);
const text = ref("");
const loading = ref(false);
const tryCreateTopic = async () => {
  loading.value = true;
  const { data } = await store.createComment({ text: text.value });
  if (data) {
    emit("created");
    text.value = "";
  }
  loading.value = false;
};
</script>

<template>
  <dm-form
    v-if="store.selectedTopic?.closed === false"
    @submit="tryCreateTopic"
    class="create_comment-container"
  >
    <dm-field>
      <dm-text
        v-model="text"
        id="text"
        placeholder="Новый комментарий"
        :rows="5"
      />
    </dm-field>
    <dm-button type="submit" label="Отправить" :loading="loading" />
  </dm-form>
  <secondary-text class="create_comment-forbidden" v-else
    >Тема закрыта, добавление комментариев невозможно</secondary-text
  >
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.create_comment-container
  margin-top: Variables.$medium

.create_comment-forbidden
  display: block
  text-align: center
</style>
