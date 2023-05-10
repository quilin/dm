import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      components: {
        menu: () => import("@/views/layout/GeneralMenu.vue"),
        sidebar: () => import("@/views/layout/GeneralSidebar.vue"),
        page: () => import("@/views/pages/home/HomePage.vue"),
      },
    },
    {
      path: "/about",
      name: "about",
      components: {
        menu: () => import("@/components/TheLoader.vue"),
        sidebar: () => import("@/components/TheLoader.vue"),
        page: () => import("@/components/TheLoader.vue"),
      },
    },
    {
      name: "rules",
      path: "/rules",
      components: {
        menu: () => import("@/components/TheLoader.vue"),
        sidebar: () => import("@/components/TheLoader.vue"),
        page: () => import("@/components/TheLoader.vue"),
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
      name: "community",
      path: "/community",
      components: {
        menu: () => import("@/components/TheLoader.vue"),
        sidebar: () => import("@/components/TheLoader.vue"),
        page: () => import("@/components/TheLoader.vue"),
      },
    },
    {
      name: "profile",
      path: "/profile/:login",
      component: () => import("@/components/TheLoader.vue"),
    },

    {
      name: "forum",
      path: "/fora/:id",
      component: () => import("@/components/TheLoader.vue"),
    },
    {
      name: "topic",
      path: "/topics/:id",
      component: () => import("@/components/TheLoader.vue"),
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
