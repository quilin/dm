<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { ref, onMounted } from "vue";
import type { Review } from "@/api/models/community";
import { useReviewStore } from "@/stores";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import BlockTitle from "@/components/layout/BlockTitle.vue";
import messages from "@/views/pages/home/RandomReview.i18n";
import { useI18n } from "vue-i18n";

const store = useReviewStore();
const { t } = useI18n({ messages });

const review = ref<Review | null>(null);
onMounted(async () => (review.value = await store.getRandomReview()));
</script>

<template>
  <block-title>{{ t("title") }}</block-title>
  <the-review v-if="review" :controls="false" :review="review" />
  <dm-loader v-else />
</template>

<style scoped lang="sass"></style>
