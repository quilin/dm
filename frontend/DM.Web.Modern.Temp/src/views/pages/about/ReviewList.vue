<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";

const route = useRoute();
const store = useReviewStore();
</script>

<template>
  <dm-paging
    v-if="store.reviews"
    :paging="store.reviews.paging!"
    :to="{ name: 'about', params: route.params }"
  />

  <dm-loader v-if="store.reviews === null" />
  <the-review
    v-else
    v-for="review in store.reviews.resources"
    :key="review.id"
    :controls="true"
    :review="review"
  />
</template>

<style scoped lang="sass"></style>
