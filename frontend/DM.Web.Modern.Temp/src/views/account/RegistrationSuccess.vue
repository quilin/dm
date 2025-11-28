<script setup lang="ts">
const emit = defineEmits<{
  (e: "confirm"): void;
}>();
</script>

<template>
  <dm-dialog>
    <page-title>Поздравляем!</page-title>
    <p>
      Вы успешно зарегистрировались на нашем сайте. Мы отправили вам письмо на
      указанный e-mail с дальнейшими инструкциями.
    </p>
    <dm-button label="Пойду проверю почту" @click="emit('confirm')" />
  </dm-dialog>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

p
  margin: Variables.$medium 0
</style>
