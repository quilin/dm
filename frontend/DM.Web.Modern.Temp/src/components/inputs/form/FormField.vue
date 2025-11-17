<template>
  <div :class="['form-field', label ? 'form-field__labeled' : null]">
    <div>
      <slot />
      <template v-if="errors">
        <div v-for="error in errors" :key="error" class="form-field-error">
          {{ error }}
        </div></template
      >
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{
  label?: string;
  name: string;
  errors?: string[];
}>();
</script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.form-field__labeled
  display: flex
  margin: Variables.$small 0
  align-items: baseline

  & input
    flex-shrink: 0

  &.error input
    animation-name: shake-error
    animation-duration: Variables.$animation-time
    animation-timing-function: ease-in-out
    +Themes.theme(border-color, Themes.$negative-border)

    &:focus
      +Themes.theme(box-shadow, Themes.$negative-border, inset 0 0 Variables.$minor)

  & label
    flex-shrink: 0
    display: inline-block
    min-width: Variables.$grid-step * 30
    +Themes.theme(color, Themes.$text)

.form-field-error
  +Themes.theme(color, Themes.$negative-text)
</style>
