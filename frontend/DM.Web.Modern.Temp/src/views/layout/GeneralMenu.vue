<script setup lang="ts">
import ForumsList from "@/views/layout/menu/ForumsList.vue";
import ModerationGames from "@/views/layout/menu/ModerationGames.vue";
import OwnGames from "@/views/layout/menu/OwnGames.vue";
import { useUserStore } from "@/stores";
import { computed } from "vue";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import GamesList from "@/views/layout/menu/GamesList.vue";
import { GameStatus } from "@/api/models/gaming";
import messages from "@/views/layout/GeneralMenu.i18n";
import { useI18n } from "vue-i18n";

const store = useUserStore();
const userIsAdmin = computed(() => userIsHighAuthority(store.user));
const { t } = useI18n({ messages });
</script>

<template>
  <template v-if="store.user">
    <moderation-games v-if="userIsAdmin" />
    <own-games />
  </template>
  <games-list
    v-else
    :title="t('activeGames')"
    :link-text="t('allActiveGames')"
    token="ActiveGames"
    :game-status="GameStatus.Active"
  />

  <games-list
    :title="t('recruitingGames')"
    :link-text="t('allRecruitingGames')"
    token="RequiringGames"
    :game-status="GameStatus.Requirement"
  />
  <games-list
    :title="t('finishedGames')"
    :link-text="t('allFinishedGames')"
    token="FinishedGames"
    :game-status="GameStatus.Finished"
  />

  <forums-list />
</template>
