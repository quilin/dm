<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { UserRole } from "@/api/models/community";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import ProfileStat from "@/views/pages/profile/ProfileStat.vue";

const route = useRoute();
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
  user.value?.roles.filter((r) => r in roleNames).map((r) => roleNames[r])
);

onMounted(() => trySelectProfile(route.params.login as string));
</script>

<template>
  <template v-if="user">
    <page-title class="profile_title">{{ route.params.login }}</page-title>
    <secondary-text class="profile_roles">{{
      userRoles.join(", ")
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
    </div>
  </template>

  <the-loader v-else :big="true" />
</template>

<style scoped lang="sass">
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
</style>
