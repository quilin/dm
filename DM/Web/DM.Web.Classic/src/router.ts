import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';

import GeneralMenu from './views/layout/GeneralMenu.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/home/Home.vue'),
      },
    },
    {
      path: '/forum/:id/:n?',
      name: 'forum',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/forum/Forum.vue'),
      },
    },
    {
      path: '/topic/:id/:n?',
      name: 'topic',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/topic/Topic.vue'),
      },
    },
  ],
});
