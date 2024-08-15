<template>
  <page-title>Сообщество</page-title>
  <router-view />
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { onMounted, watch } from "vue";
import { extractNumberParam } from "@/router";

const route = useRoute();
const { fetchUsers } = useCommunityStore();

onMounted(() => fetchUsers(extractNumberParam(route.params.n)));
watch(
  () => route.params,
  (value) => fetchUsers(extractNumberParam(value.n))
);
</script>
