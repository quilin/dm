import Vue from 'vue';
import Router from 'vue-router';

import GeneralMenu from './views/layout/GeneralMenu.vue';
import GeneralSidebar from './views/layout/GeneralSidebar.vue';

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
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/home/Home.vue'),
      },
    },
    {
      path: '/error/:code',
      name: 'error',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/error/Error.vue'),
      },
    },

    {
      path: '/about',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/reviews/Reviews.vue'),
      },
      children: [{
        path: ':n?',
        name: 'about',
        component: () => import('./views/pages/reviews/ReviewsList.vue'),
      }],
    },
    {
      path: '/rules',
      name: 'rules',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/rules/Rules.vue'),
      },
    },
    {
      path: '/community',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/community/Community.vue'),
      },
      children: [{
        path: ':n?',
        name: 'community',
        component: () => import('./views/pages/community/UsersList.vue'),
      }],
    },
    {
      path: '/fm',
      name: 'chat',
      components: {
        menu: GeneralMenu,
      },
    },
    {
      path: '/profile/:login',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/profile/Profile.vue'),
      },
      children: [{
        path: '',
        name: 'profile',
        component: () => import('./views/pages/profile/Information.vue'),
      }, {
        path: 'games',
        name: 'profile-games',
      }, {
        path: 'characters',
        name: 'profile-characters',
      }, {
        path: 'settings',
        name: 'profile-settings',
        component: () => import('./views/pages/profile/Settings.vue'),
      }],
    },

    {
      path: '/forum/:id',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/forum/Forum.vue'),
      },
      children: [{
        path: ':n',
        name: 'forum',
        component: () => import('./views/pages/forum/TopicsList.vue'),
      }],
    },
    {
      path: '/topic/:id/:n?',
      name: 'topic',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/topic/Topic.vue'),
      },
    },

    {
      path: '/create-game',
      name: 'create-game',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/create-game/CreateGame.vue'),
      },
    },
    {
      path: '/games',
      name: 'games',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
      },
    },
    {
      path: '/game/:id',
      name: 'game',
      components: {
        menu: GeneralMenu, // todo: game menu
      },
    }
  ],
});
