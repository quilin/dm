<script setup lang="ts">
import DmButton from "@/components/ui-kit/DmButton.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";
import messages from "@/views/account/RegistrationSuccess.i18n";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{
  (e: "confirmed"): void;
}>();
const { t } = useI18n({ messages });
</script>

<template>
  <dm-dialog>
    <page-title>{{ t("title") }}</page-title>
    <p>{{ t("text") }}</p>
    <dm-button :label="t('confirm')" @click="emit('confirmed')" />
  </dm-dialog>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

p
  margin: Variables.$medium 0
</style>
