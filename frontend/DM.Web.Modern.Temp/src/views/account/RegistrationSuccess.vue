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
    <the-button @click="emit('confirm')">Пойду проверю почту</the-button>
  </the-lightbox>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

p
  margin: $medium 0
</style>
