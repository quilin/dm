<script setup lang="ts">
import { ref } from "vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmProgress from "@/components/ui-kit/DmProgress.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import messages from "@/views/layout/sidebar/TheDonation.i18n";
import { useI18n } from "vue-i18n";

const { t } = useI18n({ messages });

const goal = ref(50000);
const current = ref(17000);
</script>

<template>
  <div>
    <block-title>
      <dm-icon :font="IconType.Ruble" /> {{ t("title") }}
    </block-title>
    <dm-progress :current="current" :goal="goal"
      >{{ current }} / {{ goal }} р.</dm-progress
    >
    {{ t("disclaimer1") }}<br />
    <router-link :to="{ name: 'donate' }">{{ t("link") }}</router-link>
    {{ t("disclaimer2") }}
  </div>
</template>
