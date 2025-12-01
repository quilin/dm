<script setup lang="ts" generic="T">
defineEmits(["submit"]);
</script>

<template>
  <div>
    <form @submit.prevent="$emit('submit')">
      <slot />
      <div class="controls">
        <slot name="controls" />
      </div>
    </form>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.vfm__content .controls
  display: flex
  gap: Variables.$medium
  align-items: baseline
  margin: Variables.$medium (-(Variables.$medium)) (-(Variables.$medium))
  padding: Variables.$medium
  border-radius: 0 0 Variables.$border-radius Variables.$border-radius
  +Themes.theme(background-color, Themes.$control-background)
</style>
