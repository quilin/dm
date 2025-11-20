<script setup lang="ts"></script>

<template>
  <div class="container"><slot /></div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.container
  margin: Variables.$small 0
</style>
