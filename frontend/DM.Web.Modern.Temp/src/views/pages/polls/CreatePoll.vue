<script setup lang="ts">
import { ref } from "vue";
import type { Poll } from "@/api/models/community";
import type { Post } from "@/api/models";
import { IconType } from "@/components/ui-kit/iconType";
import communityApi from "@/api/requests/communityApi";

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
    <page-title>Новый опрос</page-title>
    <dm-form @submit="tryCreatePoll">
      <dm-field>
        <dm-input
          id="poll-title"
          v-model="pollToCreate.title"
          placeholder="Вопрос"
        />
      </dm-field>
      <dm-field>
        <dm-input
          id="poll-ends"
          v-model="pollToCreate.ends"
          placeholder="Дата окончания"
        />
      </dm-field>
      <secondary-text>Ответы</secondary-text>
      <div class="create_poll-options">
        <dm-field v-for="(option, i) in pollToCreate.options" :key="i">
          <dm-input
            :id="`poll-option-${i}`"
            v-model="option.text"
            :placeholder="`Ответ #${i + 1}`"
          />
        </dm-field>
        <dm-icon-button :icon="IconType.Add" @click="addOption" />
      </div>
      <template #controls>
        <dm-button label="Создать" :loading="loading" type="submit" />
        <a @click="$emit('cancelled')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.create_poll-options
  margin-left: Variables.$small
</style>
