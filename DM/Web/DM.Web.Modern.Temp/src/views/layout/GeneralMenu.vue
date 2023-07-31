<template>
  <template v-if="store.user">
    <moderation-games v-if="userIsAdmin" />
    <own-games />
  </template>
  <games-list
    v-else
    title="Активные игры"
    link-text="Все активные игры"
    token="ActiveGames"
    :status="GameStatus.Active"
  />

  <games-list
    title="Идёт набор"
    link-text="Все игры с открытым набором"
    token="RequiringGames"
    :status="GameStatus.Requirement"
  />
  <games-list
    title="Завершённые игры"
    link-text="Все завершённые игры"
    token="FinishedGames"
    :status="GameStatus.Finished"
  />

  <forums-list />
</template>

<script setup lang="ts">
import ForumsList from "@/views/layout/menu/ForumsList.vue";
import ModerationGames from "@/views/layout/menu/ModerationGames.vue";
import OwnGames from "@/views/layout/menu/OwnGames.vue";
import { useUserStore } from "@/stores";
import { computed } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import GamesList from "@/views/layout/menu/GamesList.vue";
import { GameStatus } from "@/api/models/gaming";

const store = useUserStore();
const userIsAdmin = computed(() => userIsHighAuthority(store.user));
</script>
