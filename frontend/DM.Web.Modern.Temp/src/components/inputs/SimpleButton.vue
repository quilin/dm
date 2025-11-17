<script setup lang="ts">
import Button from "primevue/button";
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>

<template>
  <Button :loading :disabled>
    <span><slot /></span>
  </Button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Inputs"

button
  +Inputs.button()
</style>
