<template>
  <div>
    <div class="controls">TODO: элементы управления</div>
    <textarea v-model="value" :disabled="props.disabled" />
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";

const props = defineProps<{ modelValue: string; disabled: boolean }>();
const emit = defineEmits(["update:modelValue"]);

const value = ref<string>(props.modelValue);
watch(
  () => props.modelValue,
  (newValue) => {
    value.value = newValue;
    emit("update:modelValue", newValue);
  },
  { immediate: true },
);
</script>

<style scoped lang="sass">
@import "src/assets/styles/Themes"

textarea
  display: block
  height: $large
  width: 100%
  min-width: $grid-step * 100
  box-sizing: border-box
</style>
