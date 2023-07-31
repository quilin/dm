<template>
  <div class="secondary-text"><slot /></div>
</template>

<style scoped lang="sass">
.secondary-text
  font-size: $secondary-font-size
  font-weight: normal
  +theme(color, $secondary-text)
</style>
