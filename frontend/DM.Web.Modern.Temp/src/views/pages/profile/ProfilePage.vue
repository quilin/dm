<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { type UserLogin, UserRole } from "@/api/models/community";
import SecondaryText from "@/components/layout/SecondaryText.vue";
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
const isCurrentUser = computed(
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
    <page-title class="profile_title">{{ route.params.login }}</page-title>
    <secondary-text class="profile_roles">{{
      userRoles!.join(", ")
    }}</secondary-text>

    <div class="profile_container">
      <div class="profile_short-info">
        <img
          :src="user.originalPictureUrl"
          :alt="user.login"
          class="profile_short-info_picture"
        />

        <profile-stat title="Статус" empty="Не указан" v-model="user.status" />
        <profile-stat title="Имя" empty="Не указано" v-model="user.name" />
        <profile-stat
          title="Местоположение"
          empty="Не указано"
          v-model="user.location"
        />
        <profile-stat title="Skype" empty="Не указан" v-model="user.skype" />
      </div>
      <div class="profile_content">
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
            v-if="isCurrentUser"
            class="tabs-link"
            :to="{ name: 'user-settings', params: route.params }"
            >Настройки</router-link
          >
        </nav>
        <router-view />
      </div>
    </div>
  </template>

  <the-loader v-else :big="true" />
</template>

<style scoped lang="sass">
@import "src/assets/styles/Variables"

.profile_title
  display: inline-block
.profile_roles
  display: inline-block
  margin-left: $small

.profile_container
  display: flex

.profile_short-info
  width: $grid-step * 40

.profile_short-info_picture
  width: 100%
  max-height: $grid-step * 200
  border-radius: $border-radius

.profile_content
  margin-left: $big

nav
  margin-bottom: $small
  & a
    display: inline-block
    margin-right: $medium

    text-transform: uppercase
    font-weight: bold
</style>
