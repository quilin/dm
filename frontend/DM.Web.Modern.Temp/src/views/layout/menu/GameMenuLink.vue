<script setup lang="ts">
import type { Game } from "@/api/models/gaming";
import { computed } from "vue";
import { IconType } from "@/components/icons/iconType";

const props = defineProps<{
  game: Game;
  counters: boolean;
}>();
const params = computed(() => ({ id: props.game.id }));
</script>

<template>
  <div class="link">
    <router-link :to="{ name: 'game', params }">
      {{ game.title }}
    </router-link>
    <span v-if="props.counters">
      <router-link :to="{ name: 'game-first-unread-post', params }">
        <the-icon
          :font="
            game.unreadPostsCount
              ? IconType.PostsUnread
              : IconType.PostsNoUnread
          "
        />
        <template v-if="game.unreadPostsCount">{{
          props.game.unreadPostsCount
        }}</template>
      </router-link>
      <span class="separator">|</span>
      <router-link :to="{ name: 'game-comments', params }">
        <the-icon
          :font="
            game.unreadCommentsCount
              ? IconType.CommentsUnread
              : IconType.CommentsNoUnread
          "
        />
        <template v-if="game.unreadCommentsCount">{{
          game.unreadCommentsCount
        }}</template>
      </router-link>
      <template v-if="game.unreadCharactersCount">
        <span class="separator">|</span>
        <router-link :to="{ name: 'game-characters', params }">
          <the-icon :font="IconType.User" />
          {{ game.unreadCharactersCount }}
        </router-link>
      </template>
    </span>
  </div>
</template>
