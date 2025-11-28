<script setup lang="ts">
import { ref } from "vue";
import { useUserStore } from "@/stores";
import type { RegisterCredentials } from "@/api/models/account";

const emit = defineEmits<{
  (e: "success"): void;
  (e: "cancel"): void;
}>();

const credentials = ref<RegisterCredentials>({
  email: "",
  login: "",
  password: "",
});
const loading = ref(false);

const { register } = useUserStore();
const submit = async () => {
  loading.value = true;
  const badRequest = await register(credentials.value);
  loading.value = false;
  if (!badRequest) {
    emit("success");
  }
};
</script>

<template>
  <dm-dialog :with-form="true">
    <page-title>Регистрация</page-title>

    <dm-form :model="credentials" @submit="submit">
      <dm-field>
        <dm-input
          id="email"
          type="email"
          v-model="credentials.email"
          placeholder="E-mail"
        />
      </dm-field>
      <dm-field>
        <dm-input id="login" v-model="credentials.login" placeholder="Логин" />
      </dm-field>
      <dm-field>
        <dm-input
          id="password"
          type="password"
          v-model="credentials.password"
          placeholder="Пароль"
        />
      </dm-field>

      <template #controls>
        <dm-button label="Отправить" type="submit" />
        <a @click="$emit('cancel')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="stylus"></style>
