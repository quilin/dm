<script setup lang="ts">
import DmText from "@/components/ui-kit/DmText.vue";
import { ref } from "vue";
import { useForumStore } from "@/stores";

const { createComment } = useForumStore();

const emit = defineEmits(["created"]);
const text = ref("");
const tryCreateTopic = async () => {
  const { data } = await createComment({ text: text.value });
  if (data) emit("created");
  text.value = "";
};
</script>

<template>
  <dm-form @submit="tryCreateTopic">
    <dm-text v-model="text" id="text" placeholder="Новый комментарий" />
    <dm-button type="submit" label="Отправить" />
  </dm-form>
</template>

<style scoped lang="sass"></style>
