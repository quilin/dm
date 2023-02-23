import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      components: {
        menu: () => import("@/components/TheLoader.vue"),
        sidebar: () => import("@/components/TheLoader.vue"),
        page: () => import("@/components/TheLoader.vue"),
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
      name: "community",
      path: "/community",
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
      name: "create-game",
      path: "/create-game",
      component: () => import("@/components/TheLoader.vue"),
    },
  ],
});

export default router;
