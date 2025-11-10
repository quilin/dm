<template>
  <span class="secondary-text"><slot /></span>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.secondary-text
  font-size: Variables.$secondary-font-size
  font-weight: normal
  +Themes.theme(color, Themes.$secondary-text)
</style>
