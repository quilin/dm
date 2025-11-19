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

[data-pc-name="floatlabel"]
  display: inline-flex
  position: relative
  flex-grow: 1

  & label
    position: absolute
    left: Variables.$small - Variables.$tiny
    padding: 0 Variables.$tiny
    top: Variables.$small + .5px
    transform: translateY(0)
    pointer-events: none
    line-height: 1

    +Themes.theme(color, Themes.$secondary-text)
    +Themes.theme(background-color, Themes.$background)
    transition: all Variables.$animation-time !important

  &:has(:focus) label,
  label.is-filled
    top: 0
    transform: translateY(-50%)
    font-size: Variables.$secondary-font-size

textarea
  +Inputs.input()
  display: block
  height: Variables.$large
  width: 100%
  min-width: Variables.$grid-step * 100
  box-sizing: border-box
</style>
