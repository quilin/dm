<template>
  <block-title>Случайный отзыв</block-title>
  <the-review v-if="review" :controls="false" :review="review" />
  <the-loader v-else />
</template>

<script setup lang="ts">
import TheReview from "@/views/pages/about/TheReview.vue";
import { ref, onMounted } from "vue";
import communityApi from "@/api/requests/communityApi";

const review = ref();
onMounted(async () => {
  const { data } = await communityApi.getReviews({ size: 0 }, true);
  const { paging } = data!;

  const randomNumber = Math.floor(Math.random() * paging!.total);
  const { data: reviews } = await communityApi.getReviews(
    { size: 1, skip: randomNumber },
    true
  );
  const { resources } = reviews!;
  review.value = resources[0];
});
</script>

<style scoped lang="sass"></style>
