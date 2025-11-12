<script setup lang="ts">
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>

<template>
  <button :disabled="loading || disabled" :class="{ loading: loading }">
    <span><slot /></span>
  </button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Inputs"

button
  +Inputs.button()

button.loading
  background-position: center center
  background-image: url('@/assets/images/loader.gif')
  background-size: Variables.$medium
  background-repeat: no-repeat
  & span
    visibility: hidden
</style>
