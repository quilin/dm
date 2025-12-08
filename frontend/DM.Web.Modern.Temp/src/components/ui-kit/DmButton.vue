<script setup lang="ts">
import type { IconType } from "@/components/ui-kit/iconType";
import Button from "primevue/button";

defineProps<{
  label: string;
  disabled?: boolean;
  loading?: boolean;
  icon?: IconType;
}>();
</script>

<template>
  <Button
    :disabled="loading || disabled"
    :label="label"
    :pt="{
      root: 'button-root' + (loading ? ' button-root-loading' : ''),
    }"
  >
    <template v-if="icon" #icon>
      <DmIcon :font="icon" class="button-icon" />
    </template>
  </Button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Inputs"

.button-root
  +Inputs.button()

  &.button-root-loading
    background-position: center center
    background-image: url('@/assets/images/loader.gif')
    background-size: Variables.$medium
    background-repeat: no-repeat
    color: transparent !important
    cursor: progress

.button-icon
  margin-right: Variables.$minor
</style>
