<script setup lang="ts">
import { FileUpload } from "primevue";

defineModel<{ name: string }>();
</script>

<template>
  <file-upload :auto="true" :name="name" />
</template>

<style scoped lang="sass"></style>
