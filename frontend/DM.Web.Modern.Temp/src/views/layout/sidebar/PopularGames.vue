<template>
  <menu-block token="PopularGames">
    <template #title>{{ t("title") }}</template>
    <secondary-text>{{ t("empty") }}</secondary-text>
  </menu-block>
</template>

<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import messages from "@/views/layout/sidebar/PopularGames.i18n";
import { useI18n } from "vue-i18n";

const { t } = useI18n({ messages });
</script>
