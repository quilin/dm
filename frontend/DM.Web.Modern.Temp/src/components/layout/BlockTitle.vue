<template>
  <h3><slot /></h3>
</template>

<script setup lang="ts"></script>

<style scoped lang="sass">
h3
  margin: $grid-step * 5 0 $grid-step
  font-weight: bold
  text-transform: uppercase
  +theme(color, $highlight-text)
</style>
