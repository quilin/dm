<script setup lang="ts">
import { ref } from "vue";
import { useForumStore } from "@/stores";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import messages from "@/views/pages/topic/CreateComment.i18n";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{ (e: "created"): void }>();

const store = useForumStore();
const { t } = useI18n({ messages });

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
      <dm-text v-model="text" id="text" :placeholder="t('text')" :rows="5" />
    </dm-field>
    <dm-button type="submit" :label="t('submit')" :loading="loading" />
  </dm-form>
  <secondary-text class="create_comment-forbidden" v-else>{{
    t("forbidden")
  }}</secondary-text>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.create_comment-container
  margin-top: Variables.$medium

.create_comment-forbidden
  display: block
  text-align: center
</style>
