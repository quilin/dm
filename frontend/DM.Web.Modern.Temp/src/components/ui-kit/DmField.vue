<script setup lang="ts"></script>

<template>
  <div class="dm-field-container"><slot /></div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.dm-field-container
  display: flex
  margin: Variables.$small 0
</style>
