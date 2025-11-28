<script setup lang="ts" generic="T extends string | number">
import { FloatLabel } from "primevue";
import { computed } from "vue";

const props = defineProps<{
  id: string;
  placeholder?: string;
  type?: "password" | "email";
}>();
const model = defineModel<T>();

const modelType = computed(() => {
  if (props.type) return props.type;
  if (typeof model.value === "number") return "number";
  return "text";
});
const isFilled = computed(
  () => model.value !== undefined && model.value !== "",
);
</script>

<template>
  <FloatLabel>
    <input :id v-model="model" :type="modelType" />
    <label v-if="placeholder" :for="id" :class="{ 'is-filled': isFilled }">{{
      placeholder
    }}</label>
  </FloatLabel>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables.sass"
@use "@/assets/styles/Themes.sass"
@use "@/assets/styles/Inputs.sass"

+Inputs.placeholder()

/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button
  -webkit-appearance: none
  margin: 0

/* Firefox */
input[type=number]
  -moz-appearance: textfield
</style>
