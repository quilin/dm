<template>
  {{ humanTimespan }}
</template>

<script setup lang="ts">
import { computed } from "vue";
import dayjs from "dayjs";
import { useI18n } from "vue-i18n";

const props = defineProps<{ date: string }>();
const { locale } = useI18n();

const humanTimespan = computed(() =>
  dayjs(props.date).locale(locale.value).fromNow(),
);
</script>
