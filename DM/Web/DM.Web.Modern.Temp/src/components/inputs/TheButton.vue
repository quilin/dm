<template>
  <button
    :disabled="props.loading || props.disabled"
    :class="{ loading: props.loading }"
  >
    <span><slot /></span>
  </button>
</template>

<script setup lang="ts">
const props = defineProps<{ loading: boolean; disabled: boolean }>();
</script>

<style scoped lang="stylus">
button.loading
  background-position center center
  background-image url('@/assets/loader.gif')
  background-size $medium
  background-repeat no-repeat
  & span
    visibility hidden
</style>
