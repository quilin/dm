<template>
  <games-list
    title="Мои игры"
    link-text="Все активные игры"
    token="OwnGames"
    :game-status="GameStatus.Active"
  >
    <dm-loader v-if="!store.ownGames" />
    <game-menu-link
      v-else
      v-for="game in store.ownGames"
      :key="game.id"
      :game="game"
      :counters="true"
    />
  </games-list>
</template>

<script setup lang="ts">
import GameMenuLink from "@/views/layout/menu/GameMenuLink.vue";
import { useUserStore } from "@/stores";
import { useGamesStore } from "@/stores/games";
import { onMounted, watch } from "vue";
import { GameStatus } from "@/api/models/gaming";
import GamesList from "@/views/layout/menu/GamesList.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";

const userStore = useUserStore();
const store = useGamesStore();

onMounted(() => store.fetchOwnGames());
watch(
  () => userStore.user,
  () => store.fetchOwnGames(),
);
</script>
