<script setup lang="ts">
import { useRoute } from "vue-router";
import { useReviewStore } from "@/stores";
import { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";
import PageTitle from "@/components/layout/PageTitle.vue";
import messages from "@/views/pages/about/AboutPage.i18n";
import { useI18n } from "vue-i18n";

const route = useRoute();
const store = useReviewStore();
const { t } = useI18n({ messages });

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
  <page-title>{{ t("title") }}</page-title>
  <router-view />
</template>
