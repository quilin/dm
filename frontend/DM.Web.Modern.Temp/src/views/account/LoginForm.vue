<script setup lang="ts">
import { useUserStore } from "@/stores";
import { useForm } from "vee-validate";
import { object, string } from "yup";
import { ref } from "vue";
import type { LoginCredentials } from "@/api/models/account";
import { ValidationErrorCode } from "@/api/models/common";
import LightboxTitle from "@/components/layout/LightboxTitle.vue";
import TheButton from "@/components/inputs/TheButton.vue";

const emit = defineEmits<{
  (e: "success"): void;
  (e: "cancel"): void;
}>();

const { defineInputBinds, handleSubmit, meta, errorBag } =
  useForm<LoginCredentials>({
    validationSchema: object({
      login: string().required(ValidationErrorCode.Empty),
      password: string().required(ValidationErrorCode.Empty),
    }),
  });
const login = defineInputBinds("login", (state) => ({
  validateOnInput: state.errors.length > 0,
  validateOnBlur: true,
  validateOnChange: true,
}));
const password = defineInputBinds("password", (state) => ({
  validateOnInput: state.errors.length > 0,
  validateOnBlur: true,
  validateOnChange: true,
}));
const rememberMe = ref(true);

const loading = ref(false);
const { signIn } = useUserStore();
const submit = handleSubmit(async (values, { setErrors }) => {
  loading.value = true;
  const badRequest = await signIn({
    ...values,
    rememberMe: rememberMe.value,
  });
  loading.value = false;
  if (badRequest) {
    setErrors({
      login: badRequest.errors["login"] as unknown as string,
      password: badRequest.errors["password"] as unknown as string,
    });
  } else {
    emit("success");
  }
});
</script>

<template>
  <the-lightbox :with-form="true">
    <lightbox-title>Вход</lightbox-title>

    <the-form @submit="submit" :valid="meta.valid" :loading="loading">
      <form-field label="Логин" name="login" :errors="errorBag['login']">
        <input v-bind="login" id="login" />
      </form-field>
      <form-field label="Пароль" name="password" :errors="errorBag['password']">
        <input v-bind="password" type="password" id="password" />
      </form-field>
      <form-field name="rememberMe">
        <label>
          <input type="checkbox" v-model="rememberMe" />
          Запомнить меня
        </label>
      </form-field>
      <template #controls>
        <the-button type="submit" @click="submit" :loading="loading"
          >Войти</the-button
        >
        <a @click="$emit('cancel')">Отмена</a>
      </template>
    </the-form>
  </the-lightbox>
</template>
