<script setup lang="ts">
import { ref } from "vue";
import { useForumStore } from "@/stores";

import type { Topic } from "@/api/models/forum";
import type { Post } from "@/api/models";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";
import messages from "@/views/pages/forum/CreateTopic.i18n";
import { useI18n } from "vue-i18n";

const forumStore = useForumStore();
const { t } = useI18n({ messages });

const topic = ref<Post<Topic>>({
  title: "",
  description: "",
  attached: false,
  closed: false,
  forum: forumStore.selectedForum!,
});

const emit = defineEmits<{
  (e: "created", topic: Topic): void;
  (e: "cancelled"): void;
}>();

const loading = ref(false);
const tryCreateTopic = async () => {
  loading.value = true;
  const createdTopic = await forumStore.createTopic(topic.value);
  loading.value = false;
  emit("created", createdTopic);
};
</script>

<template>
  <dm-dialog :with-form="true">
    <page-title>{{ t("title") }}</page-title>
    <dm-form @submit="tryCreateTopic">
      <dm-field>
        <dm-input
          id="topic-title"
          v-model="topic.title"
          :placeholder="t('topicTitle')"
        />
      </dm-field>
      <dm-field>
        <dm-text
          id="topic-description"
          v-model="topic.description"
          :placeholder="t('description')"
        />
      </dm-field>
      <template #controls>
        <dm-button :label="t('submit')" :loading="loading" type="submit" />
        <a @click="$emit('cancelled')">{{ t("cancel") }}</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="sass"></style>
