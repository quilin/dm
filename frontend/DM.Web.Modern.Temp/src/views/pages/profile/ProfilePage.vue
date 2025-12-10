<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { type UserLogin, UserRole } from "@/api/models/community";
import ProfileStat from "@/views/pages/profile/ProfileStat.vue";
import { useUserStore } from "@/stores";
import { useFetchData } from "@/composables/useFetchData";

const route = useRoute();
const { user: currentUser } = storeToRefs(useUserStore());
const communityStore = useCommunityStore();
const { selectedUser: user } = storeToRefs(communityStore);
const { trySelectProfile } = communityStore;

const roleNames: Record<string, string> = {
  [UserRole.Administrator]: "Тролль",
  [UserRole.SeniorModerator]: "Старший гоблин",
  [UserRole.RegularModerator]: "Гоблин",
  [UserRole.NannyModerator]: "Гоблин-нянька",
};
const userRoles = computed(() =>
  user.value?.roles.filter((r) => r in roleNames).map((r) => roleNames[r]),
);
const canEdit = computed(
  () => currentUser.value && user.value?.login === currentUser.value?.login,
);

useFetchData(
  () => trySelectProfile(route.params.login as UserLogin),
  [
    {
      param: (p) => p.login,
      callback: (login) => trySelectProfile(login as UserLogin),
    },
  ],
);
</script>

<template>
  <template v-if="user">
    <page-title class="profile-title">{{ route.params.login }}</page-title>
    <secondary-text class="profile-roles">{{
      userRoles!.join(", ")
    }}</secondary-text>

    <div class="profile-container">
      <div class="profile-short_info">
        <img
          :src="user.originalPictureUrl"
          :alt="user.login"
          class="profile-short_info-picture"
        />
        <dm-upload v-if="canEdit" />

        <div>В сети: <user-online :user="user" :detailed="true" /></div>
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

.profile-short_info-picture
  width: 100%
  max-height: Variables.$grid-step * 200
  border-radius: Variables.$border-radius

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
