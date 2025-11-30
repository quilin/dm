<script setup lang="ts">
import { useUserStore } from "@/stores";
import { reactive, ref } from "vue";
import type { LoginCredentials } from "@/api/models/account";

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
  <dm-dialog :with-form="true">
    <page-title>Вход</page-title>

    <dm-form @submit="submit">
      <dm-field>
        <dm-input id="login" placeholder="Логин" v-model="credentials.login" />
      </dm-field>
      <dm-field>
        <dm-input
          id="password"
          type="password"
          v-model="credentials.password"
          placeholder="Пароль"
        />
      </dm-field>
      <dm-field>
        <label>
          <input type="checkbox" v-model="credentials.rememberMe" />
          Запомнить меня
        </label>
      </dm-field>
      <template #controls>
        <dm-button type="submit" :loading="loading" label="Войти" />
        <a @click="$emit('cancel')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>
