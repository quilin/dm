<script setup lang="ts">
import { InputText, FloatLabel } from "primevue";

const model = defineModel<string>();
defineProps<{
  id: string;
  disabled?: boolean;
  placeholder?: string;
  invalid?: boolean;
}>();
</script>

<template>
  <FloatLabel variant="on">
    <InputText
      :class="{ 'is-filled': model?.length }"
      :id="id"
      v-model="model"
      :disabled="disabled"
      :invalid
      autocomplete="off"
    />
    <label :for="id">{{ placeholder }}</label>
  </FloatLabel>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

[data-pc-name="floatlabel"]
  display: block
  position: relative

  & label
    position: absolute
    left: Variables.$small
    top: 50%
    transform: translateY(-50%)

    +Themes.theme(color, Themes.$secondary-text)
    +Themes.theme(background-color, Themes.$background)
    transition: all Variables.$animation-time !important

  &:has(input:focus) label,
  &:has(input.is-filled) label
    top: 0
    font-size: Variables.$secondary-font-size
</style>
