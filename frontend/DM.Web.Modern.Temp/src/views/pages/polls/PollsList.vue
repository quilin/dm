<script setup lang="ts">
import { usePollsStore } from "@/stores/polls";
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import ThePaging from "@/components/ThePaging.vue";
import TheLoader from "@/components/TheLoader.vue";
import ThePoll from "@/views/pages/polls/ThePoll.vue";

const route = useRoute();
const { polls } = storeToRefs(usePollsStore());
</script>

<template>
  <the-paging
    v-if="polls"
    :paging="polls.paging!"
    :to="{ name: 'polls', params: route.params }"
  />

  <the-loader v-if="!polls" :big="true" />
  <the-poll
    class="poll-container"
    v-else
    v-for="poll in polls.resources"
    :key="poll.id"
    :poll="poll"
  />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.poll-container
  margin-bottom: Variables.$big
</style>
