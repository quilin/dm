<template>
  <div>
    <block-title>
      <the-icon :font="IconType.Ruble" /> Помочь проекту
    </block-title>
    <progress-bar :current="current" :goal="goal"
      >{{ current }} / {{ goal }} р.</progress-bar
    >
    dm.am &ndash; некоммерческий проект.<br />
    <router-link :to="{ name: 'donate' }">Помогите нам</router-link> хотя бы не
    испортить его!
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import ProgressBar from "@/components/ProgressBar.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import { IconType } from "@/components/icons/iconType";

const goal = ref(50000);
const current = ref(17000);
</script>
