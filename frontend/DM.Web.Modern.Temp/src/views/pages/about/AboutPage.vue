<script setup lang="ts">
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";
import PageTitle from "@/components/layout/PageTitle.vue";

const route = useRoute();
const store = useReviewStore();

useFetchData(
  () => store.fetchReviews(extractNumberParam(route.params.n)),
  [
    {
      param: (p) => p.n,
      callback: (n) => store.fetchReviews(extractNumberParam(n)),
    },
  ],
);
</script>

<template>
  <page-title>Наши пользователи о нас</page-title>
  <router-view />
</template>
