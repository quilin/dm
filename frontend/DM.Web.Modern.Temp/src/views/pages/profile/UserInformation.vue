<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useCommunityStore } from "@/stores/community";
import TheLoader from "@/components/TheLoader.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";

const { selectedUser: user } = storeToRefs(useCommunityStore());
</script>

<template>
  <the-loader v-if="!user" :big="true" />
  <div v-else-if="user!.info" v-html="user!.info" />
  <secondary-text v-else
    >Пользователь ничего о себе не написал...</secondary-text
  >
</template>

<style scoped lang="sass"></style>
