<script setup lang="ts">
import { InputText } from "primevue";
import InputBase from "@/components/inputs/InputBase.vue";

const model = defineModel<string>();
defineProps<{
  id: string;
  type?: string;
  disabled?: boolean;
  placeholder?: string;
  invalid?: boolean;
}>();
</script>

<template>
  <InputBase :id="id" :placeholder="placeholder" :is-filled="!!model">
    <InputText
      v-model="model"
      :type="type ?? 'text'"
      :id="id"
      :disabled
      :invalid
      :pt:root="{ class: 'helpmeplz' }"
    />
  </InputBase>
</template>

<style scoped lang="sass">
.input
  background: red
</style>
