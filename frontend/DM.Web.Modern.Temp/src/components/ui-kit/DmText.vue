<script setup lang="ts">
import { FloatLabel, Textarea as TextArea } from "primevue";
import { computed } from "vue";

const model = defineModel<string>();
defineProps<{
  id: string;
  disabled?: boolean;
  placeholder?: string;
  readonly?: boolean;
}>();

const isFilled = computed(() => !!model.value);
</script>

<template>
  <div class="container">
    <div class="controls"></div>
    <float-label>
      <text-area
        :id="id"
        v-model="model"
        rows="7"
        auto-resize
        :disabled="disabled"
        :readonly="readonly"
      />
      <label v-if="placeholder" :class="{ 'is-filled': isFilled }">{{
        placeholder
      }}</label>
    </float-label>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"
@use "@/assets/styles/Inputs"

.container
  display: flex
  margin: Variables.$medium 0

+Inputs.placeholder()
[data-pc-name="floatlabel"]
  flex-grow: 1

  & label
    top: Variables.$small + .5px
    transform: translateY(0)

  &:has(:focus) label,
  label.is-filled
    transform: translateY(-50%)

textarea
  +Inputs.input()
  display: block
  height: Variables.$large
  width: 100%
  min-width: Variables.$grid-step * 100
  box-sizing: border-box
</style>
