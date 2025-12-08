<script setup lang="ts">
import type { Topic } from "@/api/models/forum";
import { ref, watch } from "vue";
import forumApi from "@/api/requests/forumApi";

const active = defineModel("active");
const props = defineProps<{ topic: Topic }>();
const emit = defineEmits(["updated"]);
const editableTopic = ref<Topic | null>(null);

watch(
  () => active.value,
  async () => {
    const { data } = await forumApi.getTopicForUpdate(props.topic.id);
    editableTopic.value = data!.resource;
  },
);

const saveChanges = async () => {
  await forumApi.updateTopic(props.topic.id, {
    title: editableTopic.value?.title,
    description: editableTopic.value?.description,
  });
  active.value = false;
  emit("updated");
};
</script>

<template>
  <dm-form v-if="active" @submit="saveChanges">
    <template v-if="editableTopic">
      <dm-field>
        <dm-input
          id="edit-topic-title"
          v-model="editableTopic.title"
          placeholder="Название"
        />
      </dm-field>
      <dm-field>
        <dm-text
          id="edit-topic-description"
          v-model="editableTopic.description"
          placeholder="Описание"
        />
      </dm-field>
      <dm-field></dm-field>
    </template>
    <dm-loader v-else />
    <template v-if="editableTopic" #controls>
      <dm-button type="submit" label="Сохранить" />
      <a @click="active = false">Отмена</a>
    </template>
  </dm-form>
</template>

<style scoped lang="sass"></style>
