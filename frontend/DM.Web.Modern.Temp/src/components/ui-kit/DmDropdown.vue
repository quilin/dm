<script setup lang="ts" generic="T">
import { Select, FloatLabel } from "primevue";
import { computed, ref, watch } from "vue";

export interface DropdownOption<T> {
  value: T;
  label: string;
  description?: string;
}

const model = defineModel<T>();
const { options, placeholder } = defineProps<{
  id: string;
  options: DropdownOption<T>[];
  placeholder?: string;
}>();

const selectedValue = ref(
  options.find((option) => option.value === model.value) ?? options[0],
);
watch(
  () => selectedValue.value,
  () => (model.value = selectedValue.value.value as T),
);
const isFilled = computed(() => !!selectedValue.value);
</script>

<template>
  <FloatLabel>
    <Select
      v-model="selectedValue"
      :options="options"
      :pt="{
        root: 'dropdown-root',
        label: 'dropdown-label',
        dropdown: 'dropdown-arrow',
        option: 'dropdown-option',
        list: 'dropdown-list',
      }"
    >
      <template #value="{ value }">{{ value?.label }}</template>
      <template #option="{ option }">{{ option.label }}</template>
    </Select>
    <label v-if="placeholder" :for="id" :class="{ 'is-filled': isFilled }">{{
      placeholder
    }}</label>
  </FloatLabel>
</template>

<style lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.dropdown-list
  +Themes.theme(background, Themes.$background)
  +Themes.theme(border, Themes.$border, 1px solid)
  border-radius: Variables.$border-radius
  list-style: none

.dropdown-option
  padding: (Variables.$minor+1) Variables.$small
  +Themes.theme(background, Themes.$background)
  cursor: pointer

  &:first-child
    border-radius: Variables.$border-radius Variables.$border-radius 0 0
  &:last-child
    border-radius: 0 0 Variables.$border-radius Variables.$border-radius
  &:hover
    +Themes.theme(background-color, Themes.$panel-background-hover)
</style>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Inputs"

+Inputs.placeholder()

.dropdown-root
  display: flex
  align-content: space-between
  min-width: 0
  flex-grow: 1
  align-items: center
  gap: Variables.$minor
  +Inputs.input()
  cursor: pointer

:deep(.dropdown-label)
  flex-grow: 1

:deep(.dropdown-arrow)
  font-size: 0
  // there's a space symbol in PrimeVue
</style>
