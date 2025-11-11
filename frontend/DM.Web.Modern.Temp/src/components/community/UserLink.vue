<template>
  <span :title="user.status">
    <router-link
      :to="{ name: 'profile', params: { login: user.login } }"
      class="user-link"
    >
      <span
        v-if="pictureSize !== 'none'"
        :style="{
          backgroundImage: pictureUrl ? `url(${pictureUrl})` : undefined,
        }"
        class="user-logo"
      />
      {{ user.login }}
    </router-link>

    <span v-if="badge" class="user-badge-container">
      [<span class="user-badge">{{ badge }}</span
      >]</span
    >
  </span>
</template>

<script setup lang="ts">
import type { User } from "@/api/models/community";
import { computed } from "vue";
import { userIsAdmin, userIsAuthority } from "@/api/models/community/helpers";

const { user, pictureSize = "small" } = defineProps<{
  user: User;
  pictureSize?: "none" | "small" | "medium";
}>();

const badge = computed(() => {
  if (userIsAdmin(user)) return "A";
  if (userIsAuthority(user)) return "M";
  return null;
});

const pictureUrl = computed(() => {
  switch (pictureSize) {
    case "small":
      return user.smallPictureUrl;
    case "medium":
      return user.mediumPictureUrl;
    default:
      return null;
  }
});
</script>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.user-link
  white-space: nowrap

.user-logo
  display: inline-block

  width: Variables.$medium
  height: Variables.$medium
  border-radius: Variables.$medium

  background: url('@/assets/images/userpic.png') 0 0 no-repeat
  vertical-align: text-bottom
  background-size: cover

.user-badge
  +Themes.theme(color, Themes.$positive-text)
</style>
