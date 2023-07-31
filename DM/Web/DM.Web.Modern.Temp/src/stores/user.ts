import { defineStore } from "pinia";
import type {
  LoginCredentials,
  RegisterCredentials,
  User,
} from "@/api/models/community";
import { ColorSchema } from "@/api/models/community";
import { ref } from "vue";
import accountApi from "@/api/requests/accountApi";
import { useUiStore } from "@/stores/ui";
import type { BadRequestError } from "@/api/models/common";

export const useUserStore = defineStore("root", () => {
  const user = ref<User | null>(null);
  const unreadConversations = ref(0);
  const userKey = "user";

  function updateUser(newUser: User | null) {
    const { updateTheme } = useUiStore();

    user.value = newUser;
    if (newUser === null) localStorage.removeItem(userKey);
    else localStorage.setItem(userKey, JSON.stringify(newUser));
    updateTheme(newUser?.settings?.colorSchema ?? ColorSchema.Modern);
  }

  async function register(credentials: RegisterCredentials) {
    const { error } = await accountApi.register(credentials);
    if (error && "errors" in error) return error as BadRequestError;
    return null;
  }

  async function signIn(credentials: LoginCredentials) {
    const { data, error } = await accountApi.signIn(credentials);
    if (data) {
      const { resource: authenticatedUser } = data;
      updateUser(authenticatedUser);
    } else if ("errors" in error!) {
      return error as BadRequestError;
    }
    return null;
  }

  async function signOut() {
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

  return { user, unreadConversations, register, signIn, signOut, fetchUser };
});
