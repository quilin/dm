<template>
  {{ humanDate }}
</template>

<script setup lang="ts">
import { computed } from "vue";
import dayjs from "dayjs";

const props = defineProps<{ date: string; format: string }>();

const humanDate = computed(() => dayjs(props.date).format(props.format));
</script>
