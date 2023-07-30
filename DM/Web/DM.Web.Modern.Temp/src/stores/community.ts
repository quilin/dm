import { defineStore } from "pinia";
import { ref } from "vue";
import type { ListEnvelope, PagingQuery } from "@/api/models/common";
import type { User } from "@/api/models/community";
import communityApi from "@/api/requests/communityApi";

export const useCommunityStore = defineStore("community", () => {
  const users = ref<ListEnvelope<User> | null>(null);

  async function fetchUsers(number: number) {
    users.value = await communityApi.getUsers({ number } as PagingQuery);
  }

  return { users, fetchUsers };
});
