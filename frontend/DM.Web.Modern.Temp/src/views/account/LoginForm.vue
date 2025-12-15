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
import messages from "@/views/account/LoginForm.i18n";
import { useI18n } from "vue-i18n";

const emit = defineEmits<{
  (e: "cancelled"): void;
}>();

const store = useUserStore();
const { t } = useI18n({ messages });

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
    <page-title>{{ t("title") }}</page-title>

    <dm-form @submit="submit">
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
      <dm-field>
        <label>
          <input type="checkbox" v-model="credentials.rememberMe" />
          {{ t("rememberMe") }}
        </label>
      </dm-field>
      <template #controls>
        <dm-button type="submit" :loading="loading" :label="t('submit')" />
        <a @click="emit('cancelled')">{{ t("cancel") }}</a>
      </template>
    </dm-form>
  </dm-dialog>
</template>
