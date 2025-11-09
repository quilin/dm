<template>
  <h3><slot /></h3>
</template>

<script setup lang="ts"></script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

h3
  margin: Variables.$grid-step * 5 0 Variables.$grid-step
  font-weight: bold
  text-transform: uppercase
  +Themes.theme(color, Themes.$highlight-text)
</style>
