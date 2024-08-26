import { defineStore } from "pinia";
import { ref } from "vue";
import type { Game } from "@/api/models/gaming";
import gamingApi from "@/api/requests/gamingApi";

export const useGamesStore = defineStore("games", () => {
  const ownGames = ref<Game[] | null>(null);
  async function fetchOwnGames() {
    const { data } = await gamingApi.getOwnGames();
    ownGames.value = data!.resources;
  }

  return { ownGames, fetchOwnGames };
});
