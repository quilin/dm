<script setup lang="ts" generic="T">
import { ref } from "vue";

defineProps<{
  model: T;
  canonicalState?: T;
  validation: (model: T) => Promise<boolean> | boolean;
}>();

const invalid = ref(false);
const disabled = ref(false);
</script>

<template>
  <form @submit.prevent="$emit('submit')">
    <slot />
    <div class="controls">
      <slot name="controls" :invalid="invalid" :disabled="disabled" />
    </div>
  </form>
</template>

<style scoped lang="sass"></style>
