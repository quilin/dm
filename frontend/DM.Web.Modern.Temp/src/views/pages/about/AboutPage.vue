<script setup lang="ts">
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";

const route = useRoute();
const { fetchReviews } = useReviewStore();

useFetchData(
  () => fetchReviews(extractNumberParam(route.params.n)),
  [
    {
      param: (p) => p.n,
      callback: (n) => fetchReviews(extractNumberParam(n)),
    },
  ]
);
</script>

<template>
  <page-title>Наши пользователи о нас</page-title>
  <router-view />
</template>
