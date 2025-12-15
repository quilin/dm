<script setup lang="ts">
import type { Comment } from "@/api/models/forum";
import { IconType } from "@/components/ui-kit/iconType";
import DmMenu, { type DmMenuItem } from "@/components/ui-kit/DmMenu.vue";

import { computed, ref } from "vue";
import { useForumStore, useUserStore } from "@/stores";
import { userIsHighAuthority } from "@/api/models/community/helpers";

import UserIcon from "@/components/community/UserIcon.vue";
import forumApi from "@/api/requests/forumApi";
import useEditMode from "@/composables/useEditMode";
import DmLike from "@/components/likes/DmLike.vue";
import { BbRenderMode } from "@/api/bbRenderMode";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import UserLink from "@/components/community/UserLink.vue";
import messages from "@/views/pages/topic/TopicComment.i18n";
import { useI18n } from "vue-i18n";

const props = defineProps<{ comment: Comment }>();

const userStore = useUserStore();
const forumStore = useForumStore();
const { t } = useI18n({ messages });

const commentActions = computed(() => {
  const result: DmMenuItem[] = [];
  if (userStore.user === null) return result;
  if (userIsHighAuthority(userStore.user)) {
    result.push({
      label: t("remove"),
      icon: IconType.Remove,
      command: removeComment,
    });
  }
  if (userStore.user.login === props.comment.author.login) {
    result.unshift({
      label: t("edit"),
      icon: IconType.Edit,
      command: initializeEditMode,
    });
  }

  return result;
});

const loading = ref(false);

const { id, isActive, acquire, release } = useEditMode();
const text = ref<string | null>(null);
const initializeEditMode = async () => {
  if (text.value === null) {
    loading.value = true;
    const { data } = await forumApi.getComment(
      props.comment.id,
      BbRenderMode.Bb,
    );
    text.value = data!.resource.text;
    loading.value = false;
  }
  acquire();
};
const updateComment = async () => {
  loading.value = true;
  await forumApi.updateComment(props.comment.id, { text: text.value! });
  loading.value = false;
  release();
  await forumStore.reloadComments();
};
const removeComment = async () => {
  loading.value = true;
  await forumApi.deleteComment(props.comment.id);
  loading.value = false;
  await forumStore.reloadComments();
};

const like = async () => {
  await forumApi.postCommentLike(props.comment.id);
  await forumStore.reloadComment(props.comment.id);
};
const unlike = async () => {
  await forumApi.deleteCommentLike(props.comment.id);
  await forumStore.reloadComment(props.comment.id);
};
</script>

<template>
  <div class="comment-container">
    <user-icon :user="comment.author" />
    <div class="comment-content">
      <div>
        <user-link :user="comment.author" :hide-picture="true" />,
        <secondary-text>
          <human-timespan :date="comment.created" /><template
            v-if="comment.updated"
          >
            (изменен <human-timespan :date="comment.updated" />)
          </template>
        </secondary-text>
      </div>

      <dm-form
        v-if="isActive"
        class="edit_comment-form"
        @submit="updateComment"
      >
        <dm-field>
          <dm-text :id="id" v-model="text" />
        </dm-field>
        <template #controls>
          <dm-button type="submit" :label="t('submit')" :loading="loading" />
          <a @click="release">{{ t("cancel") }}</a>
        </template>
      </dm-form>

      <template v-else>
        <div v-html="comment.text" />
      </template>
    </div>
    <div v-if="!isActive" class="comment-actions">
      <dm-like :entity="comment" @liked="like" @unliked="unlike" />
      <dm-menu
        v-if="commentActions.length"
        :items="commentActions"
        :loading="loading"
      />
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

$outline: Variables.$grid-step * 3

.comment-container
  display: flex
  position: relative
  margin: Variables.$small (-$outline)
  padding: $outline
  border-radius: Variables.$border-radius

.comment-actions
  display: flex
  gap: Variables.$tiny
  position: absolute
  top: $outline
  right: $outline

.comment-content
  flex-grow: 1

.edit_comment-form
  margin: (-(Variables.$small)) (-(Variables.$small)) 0
</style>
