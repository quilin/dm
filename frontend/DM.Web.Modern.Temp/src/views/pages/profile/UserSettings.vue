<script setup lang="ts">
import { reactive, ref, watch } from "vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import { useUiStore, useUserStore } from "@/stores";
import { storeToRefs } from "pinia";
import DmDropdown from "@/components/ui-kit/DmDropdown.vue";
import { ColorSchema } from "@/api/models/community";

const { user } = storeToRefs(useUserStore());
const { updateTheme } = useUiStore();

const loading = ref(false);
const settings = reactive(Object.assign({}, user.value!.settings));

watch(() => settings.colorSchema, updateTheme);
</script>

<template>
  <dm-form :model="settings" :validation="() => true">
    <div class="settings_block">
      Количество сообщений на странице
      <div class="settings_per_page">
        <dm-input
          id="postsPerPage"
          v-model="settings.paging.postsPerPage"
          class="settings_per_page-input"
        />
        &mdash; в играх
      </div>
      <div class="settings_per_page">
        <dm-input
          id="commentsPerPage"
          v-model="settings.paging.commentsPerPage"
          class="settings_per_page-input"
        />
        &mdash; в обсуждениях, новостях и на форуме
      </div>
      <div class="settings_per_page">
        <dm-input
          id="messagesPerPage"
          v-model="settings.paging.messagesPerPage"
          class="settings_per_page-input"
        />
        &mdash; в личных сообщениях
      </div>
      <div class="settings_per_page">
        <dm-input
          id="entitiesPerPage"
          v-model="settings.paging.entitiesPerPage"
          class="settings_per_page-input"
        />
        &mdash; в списках
      </div>
    </div>
    <div class="settings_block">
      <dm-dropdown
        v-model="settings.colorSchema"
        :options="[
          {
            value: ColorSchema.Night,
            label: 'Night',
          },
          {
            value: ColorSchema.Modern,
            label: 'Modern',
          },
          {
            value: ColorSchema.Classic,
            label: 'Classic',
          },
        ]"
      />
    </div>
    <dm-button label="Сохранить" :loading="loading" />
  </dm-form>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.settings_block
  margin: Variables.$medium 0

.settings_per_page
  margin: Variables.$minor 0
  padding-left: Variables.$medium

.settings_per_page-input
  width: Variables.$grid-step * 12
</style>
