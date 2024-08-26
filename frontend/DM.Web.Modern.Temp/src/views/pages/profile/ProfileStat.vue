<script setup lang="ts">
import { defineModel } from "vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";

defineProps<{ title: string; empty: string }>();
const value = defineModel();
</script>

<template>
  <dl>
    <secondary-text tag="dt">{{ title }}</secondary-text>
    <dd>{{ value || empty }}</dd>
  </dl>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

dl
  margin: $small 0

dd
  padding: 0
</style>
