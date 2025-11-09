<template>
  <div class="secondary-text"><slot /></div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.secondary-text
  font-size: Variables.$secondary-font-size
  font-weight: normal
  +Themes.theme(color, Themes.$secondary-text)
</style>
