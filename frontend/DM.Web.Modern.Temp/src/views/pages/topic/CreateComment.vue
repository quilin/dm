<script setup lang="ts">
import DmText from "@/components/ui-kit/DmText.vue";
import { ref } from "vue";
import { useForumStore } from "@/stores";
import DmField from "@/components/ui-kit/DmField.vue";
import { storeToRefs } from "pinia";
import SecondaryText from "@/components/layout/SecondaryText.vue";

const store = useForumStore();
const { selectedTopic: topic } = storeToRefs(store);

const emit = defineEmits(["created"]);
const text = ref("");
const tryCreateTopic = async () => {
  const { data } = await store.createComment({ text: text.value });
  if (data) emit("created");
  text.value = "";
};
</script>

<template>
  <dm-form
    v-if="topic?.closed === false"
    @submit="tryCreateTopic"
    class="create_comment-container"
  >
    <dm-field>
      <dm-text v-model="text" id="text" placeholder="Новый комментарий" />
    </dm-field>
    <dm-button type="submit" label="Отправить" />
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
