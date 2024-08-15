<template>
  <block-title>Случайный отзыв</block-title>
  <the-review v-if="review" :controls="false" :review="review" />
  <the-loader v-else />
</template>

<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { ref } from "vue";
import { mount } from "@vue/test-utils";
import communityApi from "@/api/requests/communityApi";
import type { PagingQuery } from "@/api/models/common";

const review = ref();
mount(async () => {
  const { paging } = await communityApi.getReviews(
    { size: 0 } as PagingQuery,
    true
  );
  const randomNumber = Math.floor(Math.random() * paging!.total);
  const { resources } = await communityApi.getReviews(
    { size: 1, skip: randomNumber } as PagingQuery,
    true
  );
  review.value = resources[0];
});
</script>

<style scoped lang="sass"></style>
