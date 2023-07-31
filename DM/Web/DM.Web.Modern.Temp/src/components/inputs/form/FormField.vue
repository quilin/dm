<template>
  <div :class="['form-field', label ? 'form-field__labeled' : null]">
    <label :for="name" v-if="label">{{ label }}</label>
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
.form-field__labeled
  display: flex
  margin: $small 0
  align-items: baseline

  & input
    flex-shrink: 0

  &.error input
    +theme(border-color, $negative-border)
    animation-name: shake-error
    animation-duration: $animation-time
    animation-timing-function: ease-in-out

    &:focus
      +theme(box-shadow, $negative-border, inset 0 0 $minor)

  & label
    flex-shrink: 0
    display: inline-block
    min-width: $grid-step * 30
    +theme(color, $text)

.form-field-error
  +theme(color, $negative-text)
</style>
