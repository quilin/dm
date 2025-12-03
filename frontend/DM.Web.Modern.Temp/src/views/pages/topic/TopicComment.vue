<script setup lang="ts">
import type { Comment } from "@/api/models/forum";
import { IconType } from "@/components/ui-kit/iconType";
import type { DmMenuItem } from "@/components/ui-kit/DmMenu.vue";

import { computed, ref } from "vue";
import { useUserStore } from "@/stores";
import { storeToRefs } from "pinia";
import { userIsHighAuthority } from "@/api/models/community/helpers";

import EditComment from "@/views/pages/topic/EditComment.vue";
import UserIcon from "@/components/community/UserIcon.vue";
import forumApi from "@/api/requests/forumApi";

const { user } = storeToRefs(useUserStore());
const { comment } = defineProps<{ comment: Comment }>();

const commentActions = computed(() => {
  const result: DmMenuItem[] = [];
  if (!user.value) return result;
  if (userIsHighAuthority(user.value)) {
    result.push({ label: "Удалить", icon: IconType.Remove, command: remove });
  }
  if (user.value.login === comment.author.login) {
    result.unshift({
      label: "Редактировать",
      icon: IconType.Edit,
      command: () => (editMode.value = true),
    });
  }

  return result;
});

const editMode = ref(false);
const remove = () => forumApi.deleteComment(comment.id);
</script>

<template>
  <div class="topic-container">
    <div class="wrapper">
      <user-icon :user="comment.author" />
      <div class="content">
        <div class="meta">
          <user-link :user="comment.author" hide-picture />,
          <secondary-text>
            <human-timespan :date="comment.created" /><template
              v-if="comment.updated"
              >, (изменен <human-timespan :date="comment.updated" />)
            </template>
          </secondary-text>
        </div>
        <edit-comment :comment="comment" v-model:active="editMode" />
        <div v-if="!editMode" v-html="comment.text" />
      </div>
      <dm-menu v-if="commentActions" class="actions" :items="commentActions" />
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

.topic-container
  margin: Variables.$small (-(Variables.$grid-step * 3))
  padding: Variables.$grid-step * 3
  border-radius: Variables.$border-radius

.wrapper
  display: flex
  position: relative

.actions
  position: absolute
  top: 0
  right: 0

.content
  position: relative
  flex-grow: 1
</style>
