<script setup lang="ts">
import PageTitle from "@/components/layout/PageTitle.vue";
import { useFetchData } from "@/composables/useFetchData";
import { usePollsStore } from "@/stores/polls";
import { extractNumberParam } from "@/router";
import { useRoute } from "vue-router";

const route = useRoute();
const { fetchPolls } = usePollsStore();

useFetchData(
  () => fetchPolls(extractNumberParam(route.params.n), false),
  [
    {
      param: (p) => p.n,
      callback: (p) => fetchPolls(extractNumberParam(p), false),
    },
  ],
);
</script>

<template>
  <page-title>Опросы</page-title>
  <router-view />
</template>
