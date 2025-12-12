<script setup lang="ts">
import { useUserStore } from "@/stores";
import { ref } from "vue";
import type { LoginCredentials } from "@/api/models/account";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";

const emit = defineEmits<{
  (e: "cancelled"): void;
}>();

const store = useUserStore();

const credentials = ref<LoginCredentials>({
  login: "",
  password: "",
  rememberMe: true,
});

const loading = ref(false);
const submit = async () => {
  loading.value = true;
  const badRequest = await store.signIn(credentials.value);
  loading.value = false;
  if (!badRequest) {
    document.location.reload();
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
        <a @click="emit('cancelled')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>
