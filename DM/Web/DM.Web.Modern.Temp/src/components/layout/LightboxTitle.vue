<template>
  <h2><slot /></h2>
</template>

<style scoped lang="sass">
h2
  margin-bottom: $small
  font-size: $title-font-size
  +theme(color, $highlight-text)
</style>
