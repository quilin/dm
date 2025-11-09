<template>
  <form @submit.prevent="submit">
    <slot />
    <div v-if="slots.controls" class="controls">
      <slot name="controls" />
    </div>
    <div v-else-if="action" class="controls">
      <the-button :disabled="valid === false" :loading="loading">{{
        action
      }}</the-button>
      <a v-if="cancel" class="controls-cancel">{{ cancel }}</a>
    </div>
  </form>
</template>

<script setup lang="ts">
import TheButton from "@/components/inputs/TheButton.vue";

defineProps<{
  valid?: boolean;
  loading?: boolean;
  action?: string;
  cancel?: string;
}>();
const emit = defineEmits(["submit"]);
const submit = () => emit("submit");
const slots = defineSlots();
</script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.controls
  margin: Variables.$small (-(Variables.$medium)) (-(Variables.$medium))
  padding: Variables.$medium
  border-radius: 0 0 Variables.$border-radius Variables.$border-radius
  +Themes.theme(background-color, Themes.$control-background)

.controls-cancel
  display: inline-block
  margin-left: Variables.$medium
</style>
