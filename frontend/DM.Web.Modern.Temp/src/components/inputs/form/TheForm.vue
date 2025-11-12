<template>
  <form @submit.prevent="$emit('submit')">
    <slot />
    <div v-if="slots.controls" class="controls"><slot name="controls" /></div>
  </form>
</template>

<script setup lang="ts">
defineProps<{
  valid?: boolean;
  loading?: boolean;
}>();
defineEmits(["submit"]);
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
</style>
