import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import HeaderDefault from "@/views/layout/HeaderDefault.vue";
import FooterDefault from "@/views/layout/FooterDefault.vue";
import MenuBarDefault from "@/views/layout/MenuBarDefault.vue";

Vue.use(VueRouter)

  const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    components: {
      header: HeaderDefault,
      footer: FooterDefault,
      menuBar: MenuBarDefault,
    },
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
