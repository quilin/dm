<script setup lang="ts">
import { ref } from "vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmProgress from "@/components/ui-kit/DmProgress.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";

const goal = ref(50000);
const current = ref(17000);
</script>

<template>
  <div>
    <block-title>
      <dm-icon :font="IconType.Ruble" /> Помочь проекту
    </block-title>
    <dm-progress :current="current" :goal="goal"
      >{{ current }} / {{ goal }} р.</dm-progress
    >
    dm.am &ndash; некоммерческий проект.<br />
    <router-link :to="{ name: 'donate' }">Помогите нам</router-link> хотя бы не
    испортить его!
  </div>
</template>
