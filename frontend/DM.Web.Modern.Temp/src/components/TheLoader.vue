<script setup lang="ts">
defineProps<{ big?: boolean }>();
</script>

<template>
  <span :class="{ loader: true, 'big-loader': big }">&nbsp;</span>
</template>

<style scoped lang="sass">
.loader
  +square($medium)
  background-image: url('@/assets/images/loader.gif')
  background-size: contain

.big-loader
  +square($big)
  margin: $big auto
</style>
