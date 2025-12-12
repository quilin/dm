<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore, useCommunityStore } from "@/stores";
import { computed, ref } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import communityApi from "@/api/requests/communityApi";
import useEditMode from "@/composables/useEditMode";
import DmButton from "@/components/ui-kit/DmButton.vue";
import DmText from "@/components/ui-kit/DmText.vue";
import DmField from "@/components/ui-kit/DmField.vue";
import DmForm from "@/components/ui-kit/DmForm.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmIconButton from "@/components/ui-kit/DmIconButton.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";

const communityStore = useCommunityStore();
const userStore = useUserStore();

const canEdit = computed(() => {
  if (communityStore.selectedUser === null || userStore.user === null)
    return false;
  if (userIsHighAuthority(communityStore.selectedUser)) return true;

  return userStore.user.login === communityStore.selectedUser.login;
});

const { isActive, acquire, release } = useEditMode();
const loading = ref(false);
const info = ref<string | null>(null);

const initializeEditMode = async () => {
  if (info.value === null) {
    loading.value = true;
    const { data } = await communityApi.getUserForUpdate(
      communityStore.selectedUser!.login,
    );
    info.value = data!.resource.info;
    loading.value = false;
  }

  acquire();
};

const saveChanges = async () => {
  await communityStore.updateUser(userStore.user!.login, {
    info: info.value!,
  });
  await communityStore.trySelectProfile(userStore.user!.login);
  release();
};
</script>

<template>
  <dm-loader v-if="!communityStore.selectedUser" :big="true" />

  <div
    class="profile-info"
    v-if="!isActive && communityStore.selectedUser !== null"
  >
    <dm-icon-button
      v-if="canEdit"
      :loading="loading"
      :icon="IconType.Edit"
      class="profile-edit_button"
      @click="initializeEditMode"
    />
    <div
      v-if="communityStore.selectedUser.info"
      v-html="communityStore.selectedUser.info"
    />
    <secondary-text v-else
      >Пользователь ничего о себе не написал...</secondary-text
    >
  </div>

  <dm-form class="profile-edit_container" v-if="isActive" @submit="saveChanges">
    <dm-field>
      <dm-text id="edit-profile-info-text" v-model="info" :rows="5" />
    </dm-field>
    <template #controls>
      <dm-button type="submit" label="Сохранить" />
      <a @click="release">Отмена</a>
    </template>
  </dm-form>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.profile-info
  position: relative
  margin: Variables.$medium 0

.profile-edit_container
  margin: 0 (-(Variables.$small + Variables.$tiny))

.profile-edit_button
  position: absolute
  top: 0
  right: 0
</style>
