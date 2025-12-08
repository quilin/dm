<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useCommunityStore } from "@/stores/community";
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore } from "@/stores";
import { computed, ref, watch } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import communityApi from "@/api/requests/communityApi";

const store = useCommunityStore();
const { selectedUser } = storeToRefs(store);
const { user: currentUser } = storeToRefs(useUserStore());

const canEdit = computed(() => {
  if (!selectedUser.value || !currentUser.value) return false;
  if (userIsHighAuthority(selectedUser.value)) return true;

  return currentUser.value.login === selectedUser.value.login;
});

const editMode = ref(false);
const info = ref<string | null>(null);
watch(
  () => editMode.value,
  async () => {
    if (info.value !== null) return;
    const { data } = await communityApi.getUserForUpdate(
      selectedUser.value!.login,
    );
    info.value = data!.resource.info;
  },
);

const saveChanges = async () => {
  await store.updateUser(currentUser.value!.login, { info: info.value! });
  await store.trySelectProfile(currentUser.value!.login);
  editMode.value = false;
};
</script>

<template>
  <dm-loader v-if="!selectedUser" :big="true" />

  <div class="profile-edit_container" v-if="canEdit">
    <!--  TODO: should be same button as activation menu link  -->
    <a v-if="!editMode" class="profile-edit_button" @click="editMode = true"
      ><dm-icon :font="IconType.Edit"
    /></a>
    <dm-form v-if="editMode" @submit="saveChanges">
      <dm-field>
        <dm-text id="edit-profile-info-text" v-model="info" />
      </dm-field>
      <template #controls>
        <dm-button type="submit" label="Сохранить" />
        <a @click="editMode = false">Отмена</a>
      </template>
    </dm-form>
  </div>

  <div class="profile-info" v-if="!editMode && selectedUser">
    <div v-if="selectedUser.info" v-html="selectedUser.info" />
    <secondary-text v-else
      >Пользователь ничего о себе не написал...</secondary-text
    >
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.profile-info
  margin: Variables.$medium 0

.profile-edit_container
  position: relative
  margin: 0 (-(Variables.$small))

.profile-edit_button
  position: absolute
  top: 0
  right: 0
</style>
