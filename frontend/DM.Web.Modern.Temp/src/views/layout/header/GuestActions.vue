<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useModal } from "vue-final-modal";
import LoginForm from "@/views/account/LoginForm.vue";
import RegistrationForm from "@/views/account/RegistrationForm.vue";
import RegistrationSuccess from "@/views/account/RegistrationSuccess.vue";

const { open: openLogin, close: closeLogin } = useModal({
  component: LoginForm,
  attrs: {
    onCancel: () => closeLogin(),
  },
});
const { open: openRegistrar, close: closeRegistrar } = useModal({
  component: RegistrationForm,
  attrs: {
    onSuccess: () => {
      closeRegistrar();
      openRegistrarSuccess();
    },
    onCancel: () => closeRegistrar(),
  },
});
const { open: openRegistrarSuccess, close: closeRegistrarSuccess } = useModal({
  component: RegistrationSuccess,
  attrs: {
    onConfirm: () => closeRegistrarSuccess(),
  },
});
</script>

<template>
  <div class="user-actions">
    <a @click="openLogin"><dm-icon :font="IconType.User" /> Вход</a>
    |
    <a @click="openRegistrar">Регистрация</a>
  </div>
</template>

<style scoped lang="sass"></style>
