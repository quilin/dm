<template>
  <span :class="{ online }">
    <template v-if="online">online</template>
    <human-timespan v-else-if="props.detailed" :date="props.user.online" />
    <span v-else class="offline">offline</span>
  </span>
</template>

<script setup lang="ts">
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import type { User } from "@/api/models/community";
import { computed } from "vue";
import dayjs from "dayjs";

const props = defineProps<{ detailed: boolean; user: User }>();

const online = computed(() => dayjs().diff(props.user.online, "m", true) < 5);
</script>

<style scoped lang="stylus">
.online
  theme(color, $positiveText)

.offline
  theme(color, $secondaryText)
</style>
