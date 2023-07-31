<script setup lang="ts">
import { ref } from "vue";
import { useForm } from "vee-validate";
import { object, string } from "yup";
import type { RegisterCredentials } from "@/api/models/community";
import { ValidationErrorCode } from "@/api/models/common";
import { useUserStore } from "@/stores";
import LightboxTitle from "@/components/layout/LightboxTitle.vue";

const emit = defineEmits<{
  (e: "success"): void;
}>();

const { handleSubmit, defineInputBinds, meta, errorBag } =
  useForm<RegisterCredentials>({
    validationSchema: object({
      email: string()
        .required(ValidationErrorCode.Empty)
        .email(ValidationErrorCode.Invalid),
      login: string()
        .required(ValidationErrorCode.Empty)
        .min(4, ValidationErrorCode.Short)
        .max(20, ValidationErrorCode.Long),
      password: string()
        .required(ValidationErrorCode.Empty)
        .min(6, ValidationErrorCode.Short),
    }),
    validateOnMount: false,
  });
const email = defineInputBinds("email");
const login = defineInputBinds("login");
const password = defineInputBinds("password");
const loading = ref(false);

const { register } = useUserStore();
const submit = handleSubmit(async (values, { setErrors }) => {
  loading.value = true;
  const badRequest = await register(values);
  loading.value = false;
  if (badRequest) {
    setErrors({
      email: badRequest.errors["email"] as unknown as string,
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
    <lightbox-title>Регистрация</lightbox-title>

    <the-form
      @submit="submit"
      :valid="meta.valid"
      :loading="loading"
      action="Зарегистрироваться"
      cancel="Отмена"
    >
      <form-field label="E-mail" name="email" :errors="errorBag['email']">
        <input v-bind="email" type="email" id="email" />
      </form-field>
      <form-field label="Логин" name="login" :errors="errorBag['login']">
        <input v-bind="login" id="login" />
      </form-field>
      <form-field label="Пароль" name="password" :errors="errorBag['password']">
        <input v-bind="password" type="password" id="password" />
      </form-field>
    </the-form>
  </the-lightbox>
</template>

<style scoped lang="stylus"></style>
