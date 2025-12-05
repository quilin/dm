<script setup lang="ts">
import { ref, watch } from "vue";
import forumApi from "@/api/requests/forumApi";
import type { Comment } from "@/api/models/forum";

const active = defineModel("active");
const props = defineProps<{ comment: Comment }>();
const emit = defineEmits(["updated"]);
const editableComment = ref<Comment | null>(null);

watch(
  () => active.value,
  async () => {
    const { data } = await forumApi.getCommentForUpdate(props.comment.id);
    editableComment.value = data!.resource;
  },
);

const saveChanges = async () => {
  await forumApi.updateComment(props.comment.id, editableComment.value!);
  active.value = false;
  emit("updated");
};
</script>

<template>
  <dm-form v-if="active" @submit="saveChanges">
    <template v-if="editableComment">
      <dm-field>
        <dm-text
          v-if="editableComment"
          id="edit-comment-text"
          v-model="editableComment.text"
        />
      </dm-field>
    </template>
    <dm-loader v-else />
    <template v-if="editableComment" #controls>
      <dm-button type="submit" label="Сохранить" />
      <a @click="active = false">Отмена</a>
    </template>
  </dm-form>
</template>

<style scoped lang="sass"></style>
