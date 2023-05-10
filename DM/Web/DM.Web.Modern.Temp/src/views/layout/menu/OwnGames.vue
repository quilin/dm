<template>
  <menu-block v-if="userStore.user" token="OwnGames">
    <template #title>Мои игры</template>

    <the-loader v-if="!store.ownGames" />
    <game-menu-link
      v-else
      v-for="game in store.ownGames"
      :key="game.id"
      :game="game"
      :counters="true"
    />

    <router-link :to="{ name: 'games', params: { status: GameStatus.Active } }">
      Все активные игры
      <the-icon :font="IconType.Forward" />
    </router-link>
  </menu-block>
</template>

<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { IconType } from "@/components/icons/iconType";
import TheLoader from "@/components/TheLoader.vue";
import GameMenuLink from "@/views/layout/menu/GameMenuLink.vue";
import { useUserStore } from "@/stores";
import { useGamesStore } from "@/stores/games";
import { onMounted, watch } from "vue";
import { GameStatus } from "@/api/models/gaming";

const userStore = useUserStore();
const store = useGamesStore();

onMounted(() => store.fetchOwnGames());
watch(
  () => userStore.user,
  () => store.fetchOwnGames()
);
</script>
