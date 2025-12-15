<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useModal } from "vue-final-modal";
import LoginForm from "@/views/account/LoginForm.vue";
import RegistrationForm from "@/views/account/RegistrationForm.vue";
import RegistrationSuccess from "@/views/account/RegistrationSuccess.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import { useI18n } from "vue-i18n";
import messages from "@/views/layout/header/GuestActions.i18n";

const { t } = useI18n({ messages });

const { open: openLogin, close: closeLogin } = useModal({
  component: LoginForm,
  attrs: {
    onCancelled: () => closeLogin(),
  },
});
const { open: openRegistrar, close: closeRegistrar } = useModal({
  component: RegistrationForm,
  attrs: {
    onRegistered: () => {
      closeRegistrar();
      openRegistrarSuccess();
    },
    onCancelled: () => closeRegistrar(),
  },
});
const { open: openRegistrarSuccess, close: closeRegistrarSuccess } = useModal({
  component: RegistrationSuccess,
  attrs: {
    onConfirmed: () => closeRegistrarSuccess(),
  },
});
</script>

<template>
  <div class="user-actions">
    <a @click="openLogin"><dm-icon :font="IconType.User" /> {{ t('signIn') }}</a>
    |
    <a @click="openRegistrar">{{ t('signOn') }}</a>
  </div>
</template>

<style scoped lang="sass"></style>
