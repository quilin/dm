<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useCommunityStore } from "@/stores/community";
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore } from "@/stores";
import { computed, ref } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import communityApi from "@/api/requests/communityApi";
import useEditMode from "@/composables/useEditMode";

const store = useCommunityStore();
const { selectedUser } = storeToRefs(store);
const { user: currentUser } = storeToRefs(useUserStore());

const canEdit = computed(() => {
  if (!selectedUser.value || !currentUser.value) return false;
  if (userIsHighAuthority(selectedUser.value)) return true;

  return currentUser.value.login === selectedUser.value.login;
});

const { isActive, acquire, release } = useEditMode();
const loading = ref(false);
const info = ref<string | null>(null);

const initializeEditMode = async () => {
  if (info.value === null) {
    loading.value = true;
    const { data } = await communityApi.getUserForUpdate(
      selectedUser.value!.login,
    );
    info.value = data!.resource.info;
    loading.value = false;
  }

  acquire();
};

const saveChanges = async () => {
  await store.updateUser(currentUser.value!.login, { info: info.value! });
  await store.trySelectProfile(currentUser.value!.login);
  release();
};
</script>

<template>
  <dm-loader v-if="!selectedUser" :big="true" />

  <div class="profile-info" v-if="!isActive && selectedUser">
    <dm-icon-button
      v-if="canEdit"
      :loading="loading"
      :icon="IconType.Edit"
      class="profile-edit_button"
      @click="initializeEditMode"
    />
    <div v-if="selectedUser.info" v-html="selectedUser.info" />
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
