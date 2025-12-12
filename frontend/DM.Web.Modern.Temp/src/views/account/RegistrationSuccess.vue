<script setup lang="ts">
import DmButton from "@/components/ui-kit/DmButton.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";

const emit = defineEmits<{
  (e: "confirmed"): void;
}>();
</script>

<template>
  <dm-dialog>
    <page-title>Поздравляем!</page-title>
    <p>
      Вы успешно зарегистрировались на нашем сайте. Мы отправили вам письмо на
      указанный e-mail с дальнейшими инструкциями.
    </p>
    <dm-button label="Пойду проверю почту" @click="emit('confirmed')" />
  </dm-dialog>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

p
  margin: Variables.$medium 0
</style>
