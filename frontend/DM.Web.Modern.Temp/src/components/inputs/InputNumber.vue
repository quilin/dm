<script setup lang="ts">
import { InputNumber } from "primevue";
import InputBase from "@/components/inputs/InputBase.vue";

const model = defineModel<number>();
defineProps<{
  id: string;
  disabled?: boolean;
  placeholder?: string;
  invalid?: boolean;
}>();
</script>

<template>
  <InputBase
    :id="id"
    :placeholder="placeholder"
    :is-filled="model !== undefined"
  >
    <InputNumber
      class="input"
      v-model.number="model"
      :id="id"
      :disabled
      :invalid
      :pt="{
        root: 'input-root',
        pcInputText: {
          root: 'input-text',
        },
      }"
    />
  </InputBase>
</template>

<style scoped lang="sass">
.input-root
  display: inline-flex
  min-width: 0

::v-deep(.input-text)
  min-width: 0
</style>
