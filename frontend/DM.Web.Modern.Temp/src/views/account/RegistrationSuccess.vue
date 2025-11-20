<script setup lang="ts">
import TheLightbox from "@/components/layout/TheLightbox.vue";
import LightboxTitle from "@/components/layout/LightboxTitle.vue";

const emit = defineEmits<{
  (e: "confirm"): void;
}>();
</script>

<template>
  <the-lightbox>
    <lightbox-title>Поздравляем!</lightbox-title>
    <p>
      Вы успешно зарегистрировались на нашем сайте. Мы отправили вам письмо на
      указанный e-mail с дальнейшими инструкциями.
    </p>
    <dm-button label="Пойду проверю почту" @click="emit('confirm')" />
  </the-lightbox>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

p
  margin: Variables.$medium 0
</style>
