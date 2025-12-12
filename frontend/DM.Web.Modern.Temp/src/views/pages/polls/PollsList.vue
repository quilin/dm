<script setup lang="ts">
import { usePollsStore } from "@/stores/polls";
import { useRoute } from "vue-router";
import ThePoll from "@/views/pages/polls/ThePoll.vue";

const route = useRoute();
const store = usePollsStore();
</script>

<template>
  <dm-paging
    v-if="store.polls"
    :paging="store.polls.paging!"
    :to="{ name: 'polls', params: route.params }"
  />

  <dm-loader v-if="store.polls === null" :big="true" />
  <the-poll
    class="poll-container"
    v-else
    v-for="poll in store.polls.resources"
    :key="poll.id"
    :poll="poll"
  />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.poll-container
  margin-bottom: Variables.$big
</style>
