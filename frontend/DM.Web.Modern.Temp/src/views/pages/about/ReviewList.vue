<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { storeToRefs } from "pinia";

const route = useRoute();
const { reviews } = storeToRefs(useReviewStore());
</script>

<template>
  <dm-paging
    v-if="reviews"
    :paging="reviews.paging!"
    :to="{ name: 'about', params: route.params }"
  />

  <dm-loader v-if="reviews === null" />
  <the-review
    v-else
    v-for="review in reviews.resources"
    :key="review.id"
    :controls="true"
    :review="review"
  />
</template>

<style scoped lang="sass"></style>
