<script setup lang="ts">
import { ColorSchema } from "@/api/models/community";
import DmDropdown, {
  type DropdownOption,
} from "@/components/ui-kit/DmDropdown.vue";

import { computed, ref, watch } from "vue";
import { useUiStore, useCommunityStore } from "@/stores";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import messages from "@/views/pages/profile/UserSettings.i18n";
import { useI18n } from "vue-i18n";

const communityStore = useCommunityStore();
const uiStore = useUiStore();
const { t } = useI18n({ messages });

const loading = ref(false);
const settings = ref(Object.assign({}, communityStore.selectedUser!.settings));

const colorSchemeOptions = computed<DropdownOption<ColorSchema>[]>(() => [
  {
    value: ColorSchema.Modern,
    label: t("colorSchemeModern"),
  },
  {
    value: ColorSchema.Classic,
    label: t("colorSchemeClassic"),
  },
  {
    value: ColorSchema.Pale,
    label: t("colorSchemePale"),
  },
  {
    value: ColorSchema.ClassicPale,
    label: t("colorSchemeClassicPale"),
  },
  {
    value: ColorSchema.Night,
    label: t("colorSchemeNight"),
  },
]);

watch(() => settings.value.colorSchema, uiStore.updateTheme);

const saveChanges = async () => {
  loading.value = true;
  await communityStore.updateUser(communityStore.selectedUser!.login, {
    settings: settings.value,
  });
  loading.value = false;
};
</script>

<template>
  <dm-form @submit="saveChanges">
    <div class="settings_block">
      {{ t("pageSize") }}
      <div class="settings_per_page">
        <dm-input
          id="postsPerPage"
          v-model="settings.paging.postsPerPage"
          class="settings_per_page-input"
        />
        &mdash; {{ t("games") }}
      </div>
      <div class="settings_per_page">
        <dm-input
          id="commentsPerPage"
          v-model="settings.paging.commentsPerPage"
          class="settings_per_page-input"
        />
        &mdash; {{ t("forum") }}
      </div>
      <div class="settings_per_page">
        <dm-input
          id="messagesPerPage"
          v-model="settings.paging.messagesPerPage"
          class="settings_per_page-input"
        />
        &mdash; {{ t("messages") }}
      </div>
      <div class="settings_per_page">
        <dm-input
          id="entitiesPerPage"
          v-model="settings.paging.entitiesPerPage"
          class="settings_per_page-input"
        />
        &mdash; {{ t("lists") }}
      </div>
    </div>
    <div class="settings_block">
      <dm-dropdown
        id="color-schema"
        class="settings_scheme"
        v-model="settings.colorSchema"
        :options="colorSchemeOptions"
        :placeholder="t('colorScheme')"
      />
    </div>
    <template #controls>
      <dm-button type="submit" :label="t('submit')" :loading="loading" />
    </template>
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

.settings_scheme
  width: Variables.$grid-step * 30
</style>
