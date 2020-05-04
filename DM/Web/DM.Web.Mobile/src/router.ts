import Vue from 'vue'
import VueRouter, {RouteConfig} from 'vue-router'
import HeaderDefault from "@/views/layout/header/HeaderDefault.vue";
import FooterDefault from "@/views/layout/footer/FooterDefault.vue";
import MenuBarDefault from "@/views/layout/menu/MenuBarDefault.vue";

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    components: {
      header: HeaderDefault,
      footer: FooterDefault,
      menuBar: MenuBarDefault,
      page: () => import('./views/pages/Home.vue'),
    },
  },
  //user
  {
    path: '/user/login',
    name: 'UserLogin',
    components: {
      header: HeaderDefault,
      footer: FooterDefault,
      menuBar: MenuBarDefault,
    },
  },
  {
    path: '/user/register',
    name: 'UserRegister',
    components: {
      header: HeaderDefault,
      footer: FooterDefault,
      menuBar: MenuBarDefault,
    },
  },
  //fora
  {
    path: '/forum/:id',
    name: 'Forum',
    components: {
      header: () => import('./views/layout/header/HeaderForum.vue'),
      footer: FooterDefault,
      menuBar: MenuBarDefault,
    }
  },
  //static pages
  {
    path: '/about',
    name: 'PageAbout',
    components: {},
  },
  {
    path: '/rules',
    name: 'PageRules',
    components: {},
  },
  {
    path: '/community',
    name: 'PageCommunity',
    components: {},
  },
  //chat
  {
    path: '/fm',
    name: 'Chat',
    components: {},
  },

]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
