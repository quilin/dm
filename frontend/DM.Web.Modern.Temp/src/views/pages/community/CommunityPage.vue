<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { extractNumberParam } from "@/router";
import { useFetchData } from "@/composables/useFetchData";

const route = useRoute();
const { fetchUsers } = useCommunityStore();

useFetchData(
  () => fetchUsers(extractNumberParam(route.params.n)),
  [
    {
      param: (p) => p.n,
      callback: (n) => fetchUsers(extractNumberParam(n)),
    },
  ],
);
</script>

<template>
  <page-title>Сообщество</page-title>
  <router-view />
</template>
