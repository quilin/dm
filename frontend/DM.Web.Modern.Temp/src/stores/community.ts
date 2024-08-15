import { defineStore } from "pinia";
import { ref } from "vue";
import type { ListEnvelope } from "@/api/models/common";
import type { User, UserLogin } from "@/api/models/community";
import communityApi from "@/api/requests/communityApi";

export const useCommunityStore = defineStore("community", () => {
  const users = ref<ListEnvelope<User> | null>(null);

  async function fetchUsers(number: number) {
    const { data } = await communityApi.getUsers({ number });
    users.value = data;
  }

  const selectedUser = ref<User | null>(null);
  async function trySelectProfile(login: UserLogin) {
    const { data, error } = await communityApi.getUser(login);
    if (error) return false;

    selectedUser.value = data!.resource;
    return true;
  }

  return { users, fetchUsers, selectedUser, trySelectProfile };
});
