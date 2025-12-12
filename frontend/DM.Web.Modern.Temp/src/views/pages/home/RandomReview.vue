<template>
  <block-title>Случайный отзыв</block-title>
  <the-review v-if="review" :controls="false" :review="review" />
  <dm-loader v-else />
</template>

<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { ref, onMounted } from "vue";
import type { Review } from "@/api/models/community";
import { useReviewStore } from "@/stores";

const store = useReviewStore();
const review = ref<Review | null>(null);
onMounted(async () => (review.value = await store.getRandomReview()));
</script>

<style scoped lang="sass"></style>
