<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { computed } from "vue";
import type { Served } from "@/api/models";
import type { User } from "@/api/models/community";
import { useUserStore } from "@/stores";
import DmIconButton from "@/components/ui-kit/DmIconButton.vue";

interface Likeable {
  likes: Served<User[]>;
  author: Served<User>;
}

const props = defineProps<{ entity: Likeable }>();
const emit = defineEmits<{
  (e: "liked"): void;
  (e: "unliked"): void;
}>();

const userStore = useUserStore();

const canInteract = computed(
  () =>
    userStore.user !== null &&
    userStore.user.login !== props.entity.author.login,
);
const userLiked = computed(() =>
  props.entity.likes.some((liker) => liker.login === userStore.user?.login),
);
</script>

<template>
  <dm-icon-button
    v-show="canInteract || entity.likes.length"
    :class="['like', canInteract && 'like-likeable', userLiked && 'like-liked']"
    :icon="IconType.Like"
    :disabled="!canInteract"
    @click="userLiked ? emit('unliked') : emit('liked')"
  >
    <template v-if="entity.likes.length">
      <span class="like-counter">{{ entity.likes.length }}</span>
    </template>
  </dm-icon-button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.like
  opacity: 0.5
  cursor: default

  &.like-likeable
    cursor: pointer
    &:hover, &.like-liked
      opacity: 1

.like-counter
  display: inline-block
  margin-left: Variables.$tiny
</style>
