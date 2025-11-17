<script setup lang="ts">
import { useUserStore } from "@/stores";
import { reactive, ref } from "vue";
import type { LoginCredentials } from "@/api/models/account";
import LightboxTitle from "@/components/layout/LightboxTitle.vue";

const emit = defineEmits<{
  (e: "success"): void;
  (e: "cancel"): void;
}>();

const credentials = reactive<LoginCredentials>({
  login: "",
  password: "",
  rememberMe: true,
});

const loading = ref(false);
const { signIn } = useUserStore();
const submit = async () => {
  loading.value = true;
  const badRequest = await signIn(credentials);
  loading.value = false;
  if (!badRequest) {
    emit("success");
  }
};
</script>

<template>
  <the-lightbox :with-form="true">
    <lightbox-title>Вход</lightbox-title>

    <the-form @submit="submit" :loading="loading">
      <form-field name="login">
        <input-text
          id="login"
          v-model="credentials.login"
          placeholder="Логин"
        />
      </form-field>
      <form-field label="Пароль" name="password">
        <input-text
          id="password"
          v-model="credentials.password"
          placeholder="Пароль"
        />
      </form-field>
      <form-field name="rememberMe">
        <label>
          <input type="checkbox" v-model="credentials.rememberMe" />
          Запомнить меня
        </label>
      </form-field>
      <template #controls>
        <simple-button type="submit" @click="submit" :loading="loading"
          >Войти</simple-button
        >
        <a @click="$emit('cancel')">Отмена</a>
      </template>
    </the-form>
  </the-lightbox>
</template>
