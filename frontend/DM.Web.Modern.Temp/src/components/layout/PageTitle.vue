<template>
  <h2><slot /></h2>
</template>

<script setup lang="ts"></script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

h2
  margin: Variables.$medium 0 Variables.$small
  font-size: Variables.$title-font-size
  +Themes.theme(color, Themes.$highlight-text)
</style>
