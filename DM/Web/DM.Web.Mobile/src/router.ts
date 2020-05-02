import Vue from 'vue'
import VueRouter, {RouteConfig} from 'vue-router'
import FooterDefault from "@/views/layout/footer/FooterDefault.vue";
import MenuBarDefault from "@/views/layout/menu/MenuBarDefault.vue";

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
    {
        path: '/',
        name: 'Home',
        components: {
            header: () => import('./views/layout/header/HeaderDefault.vue'),
            footer: FooterDefault,
            menuBar: MenuBarDefault,
        },
    },
    {
        path: '/forum/:id',
        name: 'Forum',
        components: {
            header: () => import('./views/layout/header/HeaderForum.vue'),
            footer: FooterDefault,
            menuBar: MenuBarDefault,
        }
    },
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
