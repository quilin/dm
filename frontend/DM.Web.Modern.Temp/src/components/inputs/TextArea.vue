<template>
  <div>
    <div class="controls">TODO: элементы управления</div>
    <textarea v-model="model" :disabled="props.disabled" />
  </div>
</template>

<script setup lang="ts">
const model = defineModel<string>();
const props = defineProps<{ disabled?: boolean }>();
</script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

textarea
  display: block
  height: Variables.$large
  width: 100%
  min-width: Variables.$grid-step * 100
  box-sizing: border-box
</style>
