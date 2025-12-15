<script setup lang="ts">
import PageTitle from "@/components/layout/PageTitle.vue";
import messages from "@/views/pages/rules/RulesPage.i18n";
import { useI18n } from "vue-i18n";

const { t } = useI18n({ messages });
</script>
<template>
  <page-title>{{ t("title") }}</page-title>
  <article>{{ t("text1") }}</article>
  <article>{{ t("text2") }}</article>
  <article>{{ t("text3") }}</article>
  <article>{{ t("text4") }}</article>
  <article>{{ t("text5") }}</article>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

article
  margin: Variables.$medium 0
</style>
