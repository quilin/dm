<script setup lang="ts">
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { onMounted, watch } from "vue";
import { extractNumberParam } from "@/router";

const route = useRoute();
const { fetchReviews } = useReviewStore();

onMounted(() => fetchReviews(extractNumberParam(route.params.n)));
watch(
  () => route.params,
  (value) => fetchReviews(extractNumberParam(value.n))
);
</script>

<template>
  <page-title>Наши пользователи о нас</page-title>
  <router-view />
</template>
