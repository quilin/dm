<script setup lang="ts" generic="T">
import { Select, FloatLabel } from "primevue";
import { ref, watch } from "vue";

export interface DropdownOption<T> {
  value: T;
  label: string;
  description?: string;
}

const model = defineModel<T>();
const { options, placeholder } = defineProps<{
  options: DropdownOption<T>[];
  placeholder?: string;
}>();

const selectedValue = ref(
  options.find((option) => option.value === model.value) ??
    options[0] ?? { value: undefined, label: placeholder },
);
watch(
  () => selectedValue.value,
  () => {
    console.log(selectedValue.value.value);
    model.value = selectedValue.value.value as T;
  },
);
</script>

<template>
  <FloatLabel>
    <Select v-model="selectedValue" :options="options">
      <template #value="{ value }">{{ value?.label }}</template>
      <template #option="{ option }">{{ option.label }}</template>
    </Select>
  </FloatLabel>
</template>

<style scoped lang="sass"></style>
