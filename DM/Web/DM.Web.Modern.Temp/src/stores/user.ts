import { defineStore } from "pinia";
import type { LoginCredentials, User } from "@/api/models/community";
import { ColorSchema } from "@/api/models/community";
import { ref } from "vue";
import accountApi from "@/api/requests/accountApi";
import { useUiStore } from "@/stores/ui";

export const useUserStore = defineStore("root", () => {
  const user = ref<User | null>(null);
  const unreadConversations = ref(0);
  const userKey = "user";

  function updateUser(newUser: User | null) {
    const { updateTheme } = useUiStore();

    user.value = newUser;
    if (newUser === null) localStorage.removeItem(userKey);
    else localStorage.setItem(userKey, JSON.stringify(user));
    updateTheme(newUser?.settings?.colorSchema ?? ColorSchema.Modern);
  }

  async function login(credentials: LoginCredentials) {
    const { data } = await accountApi.signIn(credentials);
    if (!data) return;

    const { resource: authenticatedUser } = data!;
    updateUser(authenticatedUser);
  }

  async function logout() {
    await accountApi.signOut();
    updateUser(null);
  }

  async function fetchUser() {
    if (!accountApi.isAuthenticated()) return;

    const serializedUser = localStorage.getItem(userKey);
    if (serializedUser) {
      console.debug("User is extracted from cache, verifying...");
      const storedUser = JSON.parse(serializedUser) as User;
      updateUser(storedUser);
    }

    const { data } = await accountApi.fetchUser();
    updateUser(data?.resource ?? null);
  }

  return { user, unreadConversations, login, logout, fetchUser };
});
