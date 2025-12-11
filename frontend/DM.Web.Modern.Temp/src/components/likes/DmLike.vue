<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { computed } from "vue";
import type { Served } from "@/api/models";
import type { User } from "@/api/models/community";
import DmIconButton from "@/components/ui-kit/DmIconButton.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";

interface Likeable {
  likes: Served<User[]>;
  author: Served<User>;
}

const props = defineProps<{
  entity: Likeable;
  user: User | null;
}>();
const emit = defineEmits(["liked", "unliked"]);

const canInteract = computed(
  () => !!props.user && props.user.login !== props.entity.author.login,
);
const userLiked = computed(() =>
  props.entity.likes.some((liker) => liker.login === props.user?.login),
);
</script>

<template>
  <dm-icon-button v-show="canInteract || entity.likes.length"
    :class="['like', canInteract && 'like-likeable', userLiked && 'like-liked']"
    :icon="IconType.Like"
    :disabled="!canInteract"
    @click="emit(userLiked ? 'unliked' : 'liked')"
  >
    <template v-if="entity.likes.length">
      <span class="like-counter">{{ entity.likes.length }}</span>
    </template>
  </dm-icon-button>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.like
  opacity: 0.7
  cursor: default

  &.like-likeable
    cursor: pointer
    &:hover, &.like-liked
      opacity: 1

.like-counter
  display: inline-block
  margin-left: Variables.$tiny
</style>
