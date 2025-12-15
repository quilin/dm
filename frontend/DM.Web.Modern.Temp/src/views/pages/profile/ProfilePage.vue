<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCommunityStore } from "@/stores/community";
import { computed, ref } from "vue";
import { type UserLogin, UserRole } from "@/api/models/community";
import ProfileStat from "@/views/pages/profile/ProfileStat.vue";
import { useUserStore } from "@/stores";
import { useFetchData } from "@/composables/useFetchData";
import DmPictureUpload from "@/components/ui-kit/DmPictureUpload.vue";
import communityApi from "@/api/requests/communityApi";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import UserOnline from "@/components/community/UserOnline.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import PageTitle from "@/components/layout/PageTitle.vue";
import messages from "@/views/pages/profile/ProfilePage.i18n";
import { useI18n } from "vue-i18n";

const route = useRoute();
const userStore = useUserStore();
const communityStore = useCommunityStore();
const { t } = useI18n({ messages });

useFetchData(
  () => communityStore.trySelectProfile(route.params.login as UserLogin),
  [
    {
      param: (p) => p.login,
      callback: (login) => communityStore.trySelectProfile(login as UserLogin),
    },
  ],
);

const roleNames = computed<Record<string, string>>(() => ({
  [UserRole.Administrator]: t("administrator"),
  [UserRole.SeniorModerator]: t("seniorModerator"),
  [UserRole.RegularModerator]: t("regularModerator"),
  [UserRole.NannyModerator]: t("nannyModerator"),
}));
const userRoles = computed(() =>
  communityStore.selectedUser?.roles
    .filter((r) => r in roleNames.value)
    .map((r) => roleNames.value[r]),
);
const canEdit = computed(
  () =>
    userStore.user !== null &&
    userStore.user.login === communityStore.selectedUser?.login,
);

const uploadProgress = ref<ProgressEvent | null>(null);
const profilePictureUrl = computed(
  () => communityStore.selectedUser?.originalPictureUrl ?? "/userpic.png",
);
const uploadPicture = async (data: FormData) => {
  await communityApi.uploadUserPicture(
    communityStore.selectedUser!.login,
    data,
    (progressEvent) => {
      uploadProgress.value = progressEvent;
    },
  );
  uploadProgress.value = null;
  await communityStore.trySelectProfile(communityStore.selectedUser!.login);
};
</script>

<template>
  <template v-if="communityStore.selectedUser">
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
            :alt="communityStore.selectedUser.login"
            :progress-event="uploadProgress"
            :picture-url="profilePictureUrl"
          />
        </div>

        <div>
          <secondary-text>{{ t("online") }}</secondary-text
          >&nbsp;
          <user-online :user="communityStore.selectedUser" :detailed="true" />
        </div>
        <profile-stat
          :title="t('status')"
          :empty="t('statusEmpty')"
          :value="communityStore.selectedUser.status"
          :update-value="(value) => ({ status: value })"
        />
        <profile-stat
          :title="t('name')"
          :empty="t('nameEmpty')"
          :value="communityStore.selectedUser.name"
          :update-value="(value) => ({ name: value })"
        />
        <profile-stat
          :title="t('location')"
          :empty="t('locationEmpty')"
          :value="communityStore.selectedUser.location"
          :update-value="(value) => ({ location: value })"
        />
        <profile-stat
          :title="t('skype')"
          :empty="t('skypeEmpty')"
          :value="communityStore.selectedUser.skype"
          :update-value="(value) => ({ skype: value })"
        />
      </div>
      <div class="profile-content">
        <nav>
          <router-link
            class="tabs-link"
            :to="{ name: 'profile', params: route.params }"
            >{{ t("info") }}</router-link
          >
          <router-link
            class="tabs-link"
            :to="{ name: 'user-games', params: route.params }"
            >{{ t("games") }}</router-link
          >
          <router-link
            class="tabs-link"
            :to="{ name: 'user-characters', params: route.params }"
            >{{ t("characters") }}</router-link
          >
          <router-link
            v-if="canEdit"
            class="tabs-link"
            :to="{ name: 'user-settings', params: route.params }"
            >{{ t("settings") }}</router-link
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
