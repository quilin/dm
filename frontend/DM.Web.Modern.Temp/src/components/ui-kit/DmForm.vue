<script setup lang="ts" generic="T">
import { ref } from "vue";

defineProps<{
  model: T;
  canonicalState?: T;
  validation?: (model: T) => Promise<boolean> | boolean;
}>();
defineEmits(["submit"]);

const invalid = ref(false);
const disabled = ref(false);
</script>

<template>
  <form @submit.prevent.stop.capture="$emit('submit')">
    <slot />
    <div class="controls">
      <slot name="controls" :invalid="invalid" :disabled="disabled" />
    </div>
  </form>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.vfm__content .controls
  display: flex
  gap: Variables.$medium
  align-items: baseline
  margin: Variables.$small (-(Variables.$medium)) (-(Variables.$medium))
  padding: Variables.$medium
  border-radius: 0 0 Variables.$border-radius Variables.$border-radius
  +Themes.theme(background-color, Themes.$control-background)
</style>
