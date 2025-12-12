<script setup lang="ts">
import { ref } from "vue";

const emit = defineEmits<{ (e: "uploadStarted", formData: FormData): void }>();

const input = ref<HTMLInputElement>();
const upload = (event: Event) => {
  const target = event.target as HTMLInputElement;
  const files = target.files;

  if (!files?.length) return;

  const formData = new FormData();
  const name = files.length > 1 ? "files" : "file";

  for (const file of files) formData.append(name, file);
  emit("uploadStarted", formData);
};
</script>

<template>
  <input type="file" class="upload-input" ref="input" @change="upload" />
</template>

<style scoped lang="sass">
.upload-input
  position: absolute
  top: 0
  right: 0
  bottom: 0
  left: 0
  padding: 0
  margin: 0
  opacity: 0
  border: none
  outline: none
  cursor: pointer
</style>
