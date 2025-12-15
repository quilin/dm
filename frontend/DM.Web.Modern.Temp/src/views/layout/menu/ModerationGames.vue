<template>
  <menu-block token="ModerationGames">
    <template #title>{{ t("title") }}</template>
  </menu-block>
</template>

<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import messages from "@/views/layout/menu/ModerationGames.i18n";
import { useI18n } from "vue-i18n";

const { t } = useI18n({ messages });
</script>
