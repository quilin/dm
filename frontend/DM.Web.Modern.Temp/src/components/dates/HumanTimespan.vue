<template>
  {{ humanTimespan }}
</template>

<script setup lang="ts">
import { computed } from "vue";
import dayjs from "dayjs";

const props = defineProps<{ date: string }>();

const humanTimespan = computed(() => dayjs(props.date).fromNow());
</script>
