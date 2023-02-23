<template>
  <div>
    <div class="title"><icon :font="$iconType.Ruble" /> Помочь проекту</div>
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

const goal = ref(50000);
const current = ref(17000);
</script>
