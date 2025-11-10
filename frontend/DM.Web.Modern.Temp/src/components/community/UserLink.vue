<template>
  <span :title="props.user.status">
    <router-link
      :to="{ name: 'profile', params: { login: props.user.login } }"
      class="user-link"
    >
      <span
        v-if="props.pictureSize !== 'none'"
        :style="{
          backgroundImage: pictureUrl ? `url(${pictureUrl})` : undefined,
        }"
        class="user-logo"
      />
      {{ props.user.login }}
    </router-link>

    <span v-if="badge" class="user-badge-container">
      [<span class="user-badge">{{ badge }}</span
      >]
    </span>
  </span>
</template>

<script setup lang="ts">
import type { User } from "@/api/models/community";
import { computed } from "vue";
import { userIsAdmin, userIsAuthority } from "@/api/models/community/helpers";

const props = defineProps<{
  user: User;
  pictureSize?: "none" | "small" | "medium";
}>();
const badge = computed(() => {
  if (userIsAdmin(props.user)) return "A";
  if (userIsAuthority(props.user)) return "M";
  return null;
});
const pictureUrl = computed(() => {
  switch (props.pictureSize) {
    case "small":
      return props.user.smallPictureUrl;
    case "medium":
      return props.user.mediumPictureUrl;
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
