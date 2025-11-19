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

[data-pc-name="floatlabel"]
  display: inline-flex
  position: relative

  & label
    position: absolute
    left: Variables.$small - Variables.$tiny
    padding: 0 Variables.$tiny
    top: 50%
    transform: translateY(-50%)
    pointer-events: none
    line-height: 1

    +Themes.theme(color, Themes.$secondary-text)
    +Themes.theme(background-color, Themes.$background)
    transition: all Variables.$animation-time !important

  &:has(input:focus) label,
  label.is-filled
    top: 0
    font-size: Variables.$secondary-font-size

  input
    min-width: 0

/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button
  -webkit-appearance: none
  margin: 0

/* Firefox */
input[type=number]
  -moz-appearance: textfield
</style>
