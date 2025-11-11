<script setup lang="ts">
import ActivePolls from "@/views/layout/sidebar/ActivePolls.vue";
import TheDonation from "@/views/layout/sidebar/TheDonation.vue";
import PopularGames from "@/views/layout/sidebar/PopularGames.vue";
import { useRoute } from "vue-router";

const route = useRoute();
</script>

<template>
  <div>
    <active-polls v-if="route.name !== 'polls'" />
    <the-donation />
    <popular-games />
  </div>
</template>
