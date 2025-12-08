<script setup lang="ts">
defineProps<{ big?: boolean }>();
</script>

<template>
  <span :class="['loader', big && 'big-loader']">&nbsp;</span>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

.loader
  +Layout.square(Variables.$medium)
  background-image: url('@/assets/images/loader.gif')
  background-size: contain
  cursor: progress

.big-loader
  +Layout.square(Variables.$big)
  margin: Variables.$big auto
</style>
