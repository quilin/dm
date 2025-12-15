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
import messages from "@/views/account/RegistrationForm.i18n";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{
  (e: "registered"): void;
  (e: "cancelled"): void;
}>();

const store = useUserStore();
const { t } = useI18n({ messages });

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
    <page-title>{{ t("title") }}</page-title>

    <dm-form @submit="submit">
      <dm-field>
        <dm-input
          id="email"
          type="email"
          v-model="credentials.email"
          :placeholder="t('email')"
        />
      </dm-field>
      <dm-field>
        <dm-input
          id="login"
          v-model="credentials.login"
          :placeholder="t('login')"
        />
      </dm-field>
      <dm-field>
        <dm-input
          id="password"
          type="password"
          v-model="credentials.password"
          :placeholder="t('password')"
        />
      </dm-field>

      <template #controls>
        <dm-button :label="t('submit')" type="submit" />
        <a @click="emit('cancelled')">{{ t("cancel") }}</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>

<style scoped lang="stylus"></style>
