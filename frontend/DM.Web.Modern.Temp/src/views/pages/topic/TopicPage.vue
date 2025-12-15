<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute, useRouter } from "vue-router";
import { useForumStore, useUserStore } from "@/stores";
import { extractNumberParam } from "@/router";
import type { Topic, TopicId } from "@/api/models/forum";
import { useFetchData } from "@/composables/useFetchData";
import { computed, ref } from "vue";
import DmMenu, { type DmMenuItem } from "@/components/ui-kit/DmMenu.vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import forumApi from "@/api/requests/forumApi";
import useEditMode from "@/composables/useEditMode";
import DmLike from "@/components/likes/DmLike.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import UserLink from "@/components/community/UserLink.vue";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import messages from "@/views/pages/topic/TopicPage.i18n";
import { useI18n } from "vue-i18n";

const route = useRoute();
const router = useRouter();
const forumStore = useForumStore();
const userStore = useUserStore();
const { t } = useI18n({ messages });

async function fetchData() {
  await forumStore.trySelectTopic(route.params.id as TopicId);
  await forumStore.fetchComments(extractNumberParam(route.params.n));
}

useFetchData(fetchData, [
  {
    param: (p) => p.id,
    callback: fetchData,
  },
  {
    param: (p) => p.n,
    callback: (n) => forumStore.fetchComments(extractNumberParam(n)),
  },
]);

const topicActions = computed(() => {
  const result: DmMenuItem[] = [];
  if (userStore.user === null) return result;

  if (userIsHighAuthority(userStore.user)) {
    result.push(
      {
        label: forumStore.selectedTopic!.closed ? t("open") : t("close"),
        icon: forumStore.selectedTopic!.closed
          ? IconType.Open
          : IconType.Closed,
        command: toggleClose,
      },
      {
        label: forumStore.selectedTopic!.attached ? t("detach") : t("attach"),
        icon: IconType.Pinned,
        command: togglePinned,
      },
      {
        label: t("remove"),
        icon: IconType.Remove,
        command: removeTopic,
      },
    );
  }
  if (userStore.user.login === forumStore.selectedTopic!.author.login) {
    result.unshift({
      label: t("edit"),
      icon: IconType.Edit,
      command: initializeEditMode,
    });
  }
  return result;
});

const { isActive, acquire, release } = useEditMode();
const loading = ref(false);

const topicToEdit = ref<Topic | null>(null);
const initializeEditMode = async () => {
  if (topicToEdit.value === null) {
    loading.value = true;
    const { data } = await forumApi.getTopicForUpdate(
      forumStore.selectedTopic!.id,
    );
    topicToEdit.value = data!.resource;
    loading.value = false;
  }
  acquire();
};
const updateTopic = async () => {
  loading.value = true;
  await forumApi.updateTopic(forumStore.selectedTopic!.id, {
    title: topicToEdit.value!.title,
    description: topicToEdit.value!.description,
  });
  loading.value = false;
  await forumStore.trySelectTopic(forumStore.selectedTopic!.id);
};

const toggleClose = async () => {
  loading.value = true;
  await forumApi.updateTopic(forumStore.selectedTopic!.id, {
    closed: !forumStore.selectedTopic!.closed,
  });
  forumStore.selectedTopic!.closed = !forumStore.selectedTopic!.closed;
  loading.value = false;
};
const togglePinned = async () => {
  loading.value = true;
  await forumApi.updateTopic(forumStore.selectedTopic!.id, {
    attached: !forumStore.selectedTopic!.attached,
  });
  forumStore.selectedTopic!.attached = !forumStore.selectedTopic!.attached;
  loading.value = false;
};
const removeTopic = async () => {
  loading.value = true;
  await forumApi.deleteTopic(forumStore.selectedTopic!.id);
  loading.value = false;
  await router.push({
    name: "forum",
    params: { n: undefined, id: forumStore.selectedTopic!.forum.id },
  });
};

const like = async () => {
  await forumApi.postTopicLike(forumStore.selectedTopic!.id);
  await forumStore.trySelectTopic(forumStore.selectedTopic!.id);
};
const unlike = async () => {
  await forumApi.deleteTopicLike(forumStore.selectedTopic!.id);
  await forumStore.trySelectTopic(forumStore.selectedTopic!.id);
};
</script>

<template>
  <template v-if="forumStore.selectedTopic !== null">
    <div class="topic-header">
      <page-title>{{ forumStore.selectedTopic.title }}</page-title>
      <router-link
        :to="{
          name: 'forum',
          params: { id: forumStore.selectedTopic.forum.id },
        }"
      >
        <dm-icon :font="IconType.ArrowLeft" />
        {{t("backToForum")}} "{{ forumStore.selectedTopic.forum.id }}"
      </router-link>
    </div>
    <div class="topic-content">
      <dm-form v-if="isActive" @submit="updateTopic">
        <dm-field>
          <dm-input
            id="edit-topic-title"
            v-model="topicToEdit!.title"
            :placeholder="t('title')"
          />
        </dm-field>
        <dm-field>
          <dm-text
            id="edit-topic-description"
            v-model="topicToEdit!.description"
            :placeholder="t('description')"
          />
        </dm-field>
        <template #controls>
          <dm-button type="submit" :label="t('submit')" />
          <a @click="release">{{t("cancel")}}</a>
        </template>
      </dm-form>
      <template v-else>
        <div
          v-if="forumStore.selectedTopic.description"
          class="topic-description"
          v-html="forumStore.selectedTopic.description"
        />
        <user-link :user="forumStore.selectedTopic.author" />,
        <secondary-text
          ><human-timespan :date="forumStore.selectedTopic.created"
        /></secondary-text>
        <div class="topic-actions">
          <dm-like
            :entity="forumStore.selectedTopic"
            @liked="like"
            @unliked="unlike"
          />
          <dm-menu
            v-if="topicActions.length"
            :items="topicActions"
            :loading="loading"
          />
        </div>
      </template>
    </div>
  </template>
  <dm-loader v-else :big="true" />
  <router-view />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.topic-header
  display: flex
  justify-content: space-between
  align-items: baseline

.topic-content
  position: relative

.topic-actions
  display: flex
  gap: Variables.$tiny
  position: absolute
  bottom: 0
  right: 0

.topic-description
  margin-bottom: Variables.$minor
</style>
