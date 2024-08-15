<script setup lang="ts">
import ThePaging from "@/components/ThePaging.vue";
import TheReview from "@/views/pages/about/TheReview.vue";
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { storeToRefs } from "pinia";

const route = useRoute();
const { reviews } = storeToRefs(useReviewStore());
</script>

<template>
  <the-paging
    v-if="reviews"
    :paging="reviews.paging!"
    :to="{ name: 'about', params: route.params }"
  />

  <the-loader v-if="reviews === null" />
  <the-review
    v-else
    v-for="review in reviews.resources"
    :key="review.id"
    :controls="true"
    :review="review"
  />
</template>

<style scoped lang="sass"></style>
