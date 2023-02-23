<template>
  <div :class="{ loader: true, 'big-loader': props.big }">&nbsp;</div>
</template>

<script setup lang="ts">
const props = defineProps<{ big?: boolean }>();
</script>

<style scoped lang="sass">
.loader
  +square($medium)
  background-image: url('@/assets/images/loader.gif')
  background-size: contain

.big-loader
  +square($big)
  margin: $big auto
</style>
