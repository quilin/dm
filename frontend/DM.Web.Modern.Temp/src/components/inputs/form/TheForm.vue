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
import { defineSlots } from "vue";
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
.controls
  margin: $small (-$medium) (-$medium)
  padding: $medium
  +theme(background-color, $control-background)
  border-radius: 0 0 $border-radius $border-radius

.controls-cancel
  display: inline-block
  margin-left: $medium
</style>
