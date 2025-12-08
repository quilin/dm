<script setup lang="ts">
import type { Comment } from "@/api/models/forum";
import { IconType } from "@/components/ui-kit/iconType";
import type { DmMenuItem } from "@/components/ui-kit/DmMenu.vue";

import { computed, ref } from "vue";
import { useForumStore, useUserStore } from "@/stores";
import { storeToRefs } from "pinia";
import { userIsHighAuthority } from "@/api/models/community/helpers";

import UserIcon from "@/components/community/UserIcon.vue";
import forumApi from "@/api/requests/forumApi";
import useEditMode from "@/composables/useEditMode";

const { user } = storeToRefs(useUserStore());
const { reloadComments } = useForumStore();
const { comment } = defineProps<{ comment: Comment }>();

const commentActions = computed(() => {
  const result: DmMenuItem[] = [];
  if (!user.value) return result;
  if (userIsHighAuthority(user.value)) {
    result.push({
      label: "Удалить",
      icon: IconType.Remove,
      command: removeComment,
    });
  }
  if (user.value.login === comment.author.login) {
    result.unshift({
      label: "Редактировать",
      icon: IconType.Edit,
      command: initializeEditMode,
    });
  }

  return result;
});

const loading = ref(false);

const { isActive, acquire, release } = useEditMode();
const text = ref<string | null>(null);
const initializeEditMode = async () => {
  if (text.value === null) {
    loading.value = true;
    const { data } = await forumApi.getCommentForUpdate(comment.id);
    text.value = data!.resource.text;
    loading.value = false;
  }
  acquire();
};
const updateComment = async () => {
  loading.value = true;
  await forumApi.updateComment(comment.id, { text: text.value! });
  loading.value = false;
  release();
  await reloadComments();
};
const removeComment = async () => {
  loading.value = true;
  await forumApi.deleteComment(comment.id);
  loading.value = false;
  await reloadComments();
};
</script>

<template>
  <div class="comment-container">
    <div class="comment-wrapper">
      <user-icon :user="comment.author" />
      <div class="comment-content">
        <div>
          <user-link :user="comment.author" :hide-picture="true" />,
          <secondary-text>
            <human-timespan :date="comment.created" /><template
              v-if="comment.updated"
              >, (изменен <human-timespan :date="comment.updated" />)
            </template>
          </secondary-text>
        </div>

        <dm-form
          v-if="isActive"
          class="edit_comment-form"
          @submit="updateComment"
        >
          <dm-field>
            <dm-text id="edit_comment-text" v-model="text" />
          </dm-field>
          <template #controls>
            <dm-button type="submit" label="Сохранить" :loading="loading" />
            <a @click="release">Отмена</a>
          </template>
        </dm-form>

        <template v-else>
          <div v-html="comment.text" />
          <dm-menu
            v-if="commentActions"
            class="comment-actions"
            :items="commentActions"
            :loading="loading"
          />
        </template>
      </div>
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

.comment-container
  margin: Variables.$small (-(Variables.$grid-step * 3))
  padding: Variables.$grid-step * 3
  border-radius: Variables.$border-radius

.comment-wrapper
  display: flex

.comment-actions
  position: absolute
  top: 0
  right: 0

.comment-content
  position: relative
  flex-grow: 1

.edit_comment-form
  margin: (-(Variables.$small)) (-(Variables.$small)) 0
</style>
