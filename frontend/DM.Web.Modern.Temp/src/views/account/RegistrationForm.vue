<script setup lang="ts">
import { ref } from "vue";
import { useUserStore } from "@/stores";
import type { RegisterCredentials } from "@/api/models/account";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import DmDialog from "@/components/ui-kit/DmDialog.vue";

const emit = defineEmits<{
  (e: "registered"): void;
  (e: "cancelled"): void;
}>();
const store = useUserStore();

const credentials = ref<RegisterCredentials>({
  email: "",
  login: "",
  password: "",
});
const loading = ref(false);

const submit = async () => {
  loading.value = true;
  const badRequest = await store.register(credentials.value);
  loading.value = false;
  if (!badRequest) {
    emit("registered");
  }
};
</script>

<template>
  <dm-dialog :with-form="true">
    <page-title>Регистрация</page-title>

    <dm-form @submit="submit">
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
        <a @click="emit('cancelled')">Отмена</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="stylus"></style>
