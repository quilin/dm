<script setup lang="ts">
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import type { User } from "@/api/models/community";
import { computed } from "vue";
import dayjs from "dayjs";

const props = defineProps<{ detailed: boolean; user: User }>();
const online = computed(() => dayjs().diff(props.user.online, "m", true) < 5);
</script>

<template>
  <span :class="{ online }">
    <template v-if="online">online</template>
    <human-timespan v-else-if="detailed" :date="user.online" />
    <span v-else class="offline">offline</span>
  </span>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Themes"

.online
  +theme(color, $positive-text)

.offline
  +theme(color, $secondary-text)
</style>
