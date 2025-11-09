<template>
  <h2><slot /></h2>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

h2
  margin-bottom: Variables.$small
  font-size: Variables.$title-font-size
  +Themes.theme(color, Themes.$highlight-text)
</style>
