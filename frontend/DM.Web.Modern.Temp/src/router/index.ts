import { createRouter, createWebHistory } from "vue-router";

import GeneralMenu from "@/views/layout/GeneralMenu.vue";
import GeneralSidebar from "@/views/layout/GeneralSidebar.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/home/HomePage.vue"),
      },
    },
    {
      path: "/about",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/about/AboutPage.vue"),
      },
      children: [
        {
          name: "about",
          path: ":n?",
          component: () => import("@/views/pages/about/ReviewList.vue"),
        },
      ],
    },
    {
      name: "rules",
      path: "/rules",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/rules/RulesPage.vue"),
      },
    },
    {
      name: "chat",
      path: "/chat",
      component: () => import("@/components/TheLoader.vue"),
    },
    {
      name: "messenger",
      path: "/messenger",
      component: () => import("@/components/TheLoader.vue"),
    },
    {
      name: "donate",
      path: "/donate",
      component: () => import("@/components/TheLoader.vue"),
    },

    {
      path: "/community",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/community/CommunityPage.vue"),
      },
      children: [
        {
          name: "community",
          path: ":n?",
          component: () => import("@/views/pages/community/UsersList.vue"),
        },
      ],
    },
    {
      path: "/profile",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/profile/ProfilePage.vue"),
      },
      children: [
        {
          name: "profile",
          path: "/:login",
          component: () => import("@/views/pages/profile/UserInformation.vue"),
        },
        {
          name: "user-games",
          path: "/:login/games",
          component: () => import("@/views/pages/profile/UserGames.vue"),
        },
        {
          name: "user-characters",
          path: "/:login/characters",
          component: () => import("@/views/pages/profile/UserCharacters.vue"),
        },
        {
          name: "user-settings",
          path: "/:login/settings",
          component: () => import("@/views/pages/profile/UserSettings.vue"),
        },
      ],
    },

    {
      path: "/fora/:id",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/forum/ForumPage.vue"),
      },
      children: [
        {
          name: "forum",
          path: ":n?",
          component: () => import("@/views/pages/forum/TopicsList.vue"),
        },
      ],
    },
    {
      path: "/topics/:id",
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import("@/views/pages/topic/TopicPage.vue"),
      },
      children: [
        {
          name: "topic",
          path: ":n?",
          component: () => import("@/views/pages/topic/CommentsList.vue"),
        },
      ],
    },

    {
      name: "create-game",
      path: "/create-game",
      component: () => import("@/components/TheLoader.vue"),
    },
    {
      name: "games",
      path: "/games/:status",
      component: () => import("@/components/TheLoader.vue"),
    },
    {
      name: "game",
      path: "/games/:id",
      component: () => import("@/components/TheLoader.vue"),
    },
  ],
});

export default router;

export function extractNumberParam(
  param: string | string[],
  defaultValue: number = 1,
) {
  return parseInt(param as string) || defaultValue;
}
