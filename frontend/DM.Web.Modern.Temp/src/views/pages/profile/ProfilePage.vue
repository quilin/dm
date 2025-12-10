<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { storeToRefs } from "pinia";
import { computed, ref } from "vue";
import { type UserLogin, UserRole } from "@/api/models/community";
import ProfileStat from "@/views/pages/profile/ProfileStat.vue";
import { useUserStore } from "@/stores";
import { useFetchData } from "@/composables/useFetchData";
import DmPictureUpload from "@/components/ui-kit/DmPictureUpload.vue";
import communityApi from "@/api/requests/communityApi";
import SecondaryText from "@/components/layout/SecondaryText.vue";

const route = useRoute();
const { user: currentUser } = storeToRefs(useUserStore());
const communityStore = useCommunityStore();
const { selectedUser: user } = storeToRefs(communityStore);
const { trySelectProfile } = communityStore;

useFetchData(
  () => trySelectProfile(route.params.login as UserLogin),
  [
    {
      param: (p) => p.login,
      callback: (login) => trySelectProfile(login as UserLogin),
    },
  ],
);

const roleNames: Record<string, string> = {
  [UserRole.Administrator]: "Тролль",
  [UserRole.SeniorModerator]: "Старший гоблин",
  [UserRole.RegularModerator]: "Гоблин",
  [UserRole.NannyModerator]: "Гоблин-нянька",
};
const userRoles = computed(() =>
  user.value?.roles.filter((r) => r in roleNames).map((r) => roleNames[r]),
);
const canEdit = computed(() => {
  if (currentUser.value === null) return false;
  return user.value?.login === currentUser.value?.login;
});

const uploadProgress = ref<ProgressEvent | null>(null);
const profilePictureUrl = computed(
  () => user.value?.originalPictureUrl ?? "/userpic.png",
);
const uploadPicture = async (data: FormData) => {
  await communityApi.uploadUserPicture(
    user.value!.login,
    data,
    (progressEvent) => {
      uploadProgress.value = progressEvent;
    },
  );
  uploadProgress.value = null;
  await trySelectProfile(user.value!.login);
};
</script>

<template>
  <template v-if="user">
    <page-title class="profile-title">{{ route.params.login }}</page-title>
    <secondary-text class="profile-roles">{{
      userRoles!.join(", ")
    }}</secondary-text>

    <div class="profile-container">
      <div class="profile-short_info">
        <div class="profile-picture">
          <dm-picture-upload
            @upload="uploadPicture"
            :can-upload="canEdit"
            :alt="user.login"
            :progress-event="uploadProgress"
            :picture-url="profilePictureUrl"
          />
        </div>

        <div>
          <secondary-text>В сети</secondary-text>&nbsp;
          <user-online :user="user" :detailed="true" />
        </div>
        <profile-stat
          title="Статус"
          empty="Не указан"
          :value="user.status"
          :update-value="(value) => ({ status: value })"
        />
        <profile-stat
          title="Имя"
          empty="Не указано"
          :value="user.name"
          :update-value="(value) => ({ name: value })"
        />
        <profile-stat
          title="Местоположение"
          empty="Не указано"
          :value="user.location"
          :update-value="(value) => ({ location: value })"
        />
        <profile-stat
          title="Skype"
          empty="Не указан"
          :value="user.skype"
          :update-value="(value) => ({ skype: value })"
        />
      </div>
      <div class="profile-content">
        <nav>
          <router-link
            class="tabs-link"
            :to="{ name: 'profile', params: route.params }"
            >Информация</router-link
          >
          <router-link
            class="tabs-link"
            :to="{ name: 'user-games', params: route.params }"
            >Игры</router-link
          >
          <router-link
            class="tabs-link"
            :to="{ name: 'user-characters', params: route.params }"
            >Персонажи</router-link
          >
          <router-link
            v-if="canEdit"
            class="tabs-link"
            :to="{ name: 'user-settings', params: route.params }"
            >Настройки</router-link
          >
        </nav>
        <router-view />
      </div>
    </div>
  </template>

  <dm-loader v-else :big="true" />
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.profile-title
  display: inline-block
.profile-roles
  display: inline-block
  margin-left: Variables.$small

.profile-container
  display: flex
  gap: Variables.$big

.profile-short_info
  width: Variables.$grid-step * 50
  flex-shrink: 0

.profile-picture
  margin-bottom: Variables.$small

.profile-content
  flex-grow: 1
  min-width: 0

nav
  margin-bottom: Variables.$small
  & a
    display: inline-block
    margin-right: Variables.$medium

    text-transform: uppercase
    font-weight: bold
</style>
