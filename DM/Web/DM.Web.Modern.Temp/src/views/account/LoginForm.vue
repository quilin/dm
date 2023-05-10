<template>
  <the-lightbox>
    <page-title>Вход</page-title>
    <form @submit.prevent="submit">
      <input v-bind="login" />{{ errors.login }}<br />
      <input type="password" v-bind="password" />{{ errors.password }}<br />
      <input type="checkbox" v-model="rememberMe" /><br />
      <the-button :disabled="!meta.valid" :loading="loading">Войти</the-button>
    </form>
  </the-lightbox>
</template>

<script setup lang="ts">
import PageTitle from "@/components/layout/PageTitle.vue";
import { useUserStore } from "@/stores";
import { useForm } from "vee-validate";
import { object, string } from "yup";
import { ref } from "vue";
import type { LoginCredentials } from "@/api/models/community";

const emit = defineEmits<{
  (e: "login"): void;
}>();

const { defineInputBinds, handleSubmit, errors, meta } =
  useForm<LoginCredentials>({
    validationSchema: object({
      login: string().required("required"),
      password: string().required("required"),
    }),
  });
const login = defineInputBinds("login", (state) => ({
  validateOnInput: state.errors.length > 0,
  validateOnBlur: true,
}));
const password = defineInputBinds("password", (state) => ({
  validateOnInput: state.errors.length > 0,
  validateOnBlur: true,
}));
const rememberMe = ref(true);

const loading = ref(false);
const submit = handleSubmit(async (values, { setErrors }) => {
  const { signIn } = useUserStore();
  loading.value = true;
  const badRequest = await signIn({
    ...values,
    rememberMe: rememberMe.value,
  });
  loading.value = false;
  if (badRequest) {
    // todo: translations
    setErrors({
      login: badRequest.errors["login"]?.join(";"),
      password: badRequest.errors["password"]?.join(";"),
    });
  } else {
    emit("login");
  }
});
</script>

<style scoped lang="stylus">
.vfm
  display flex;

.lightbox
  width $large

  & h2
    margin-top: 0
</style>
