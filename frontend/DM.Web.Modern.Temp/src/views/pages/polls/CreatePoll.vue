<script setup lang="ts">
import { ref } from "vue";
import type { Poll } from "@/api/models/community";
import type { Post } from "@/api/models";
import { IconType } from "@/components/ui-kit/iconType";
import communityApi from "@/api/requests/communityApi";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmIconButton from "@/components/ui-kit/DmIconButton.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";
import messages from "@/views/pages/polls/CreatePoll.i18n";
import { useI18n } from "vue-i18n";

const { t } = useI18n({ messages });

const pollToCreate = ref<Post<Poll>>({
  title: "",
  ends: new Date().toISOString(),
  options: [{ text: "" }],
});
const addOption = () => {
  pollToCreate.value.options.push({ text: "" });
};

const loading = ref(false);
const tryCreatePoll = async () => {
  loading.value = true;
  await communityApi.postPoll(pollToCreate.value);
  loading.value = false;
};
</script>

<template>
  <dm-dialog :with-form="true">
    <page-title>{{ t("title") }}</page-title>
    <dm-form @submit="tryCreatePoll">
      <dm-field>
        <dm-input
          id="poll-title"
          v-model="pollToCreate.title"
          :placeholder="t('question')"
        />
      </dm-field>
      <dm-field>
        <dm-input
          id="poll-ends"
          v-model="pollToCreate.ends"
          :placeholder="t('ends')"
        />
      </dm-field>
      <secondary-text>Ответы</secondary-text>
      <div class="create_poll-options">
        <dm-field v-for="(option, i) in pollToCreate.options" :key="i">
          <dm-input
            :id="`poll-option-${i}`"
            v-model="option.text"
            :placeholder="t('answer', { number: i + 1 })"
          />
        </dm-field>
        <dm-icon-button :icon="IconType.Add" @click="addOption" />
      </div>
      <template #controls>
        <dm-button :label="t('submit')" :loading="loading" type="submit" />
        <a @click="$emit('cancelled')">{{ t("cancel") }}</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.create_poll-options
  margin-left: Variables.$small
</style>
