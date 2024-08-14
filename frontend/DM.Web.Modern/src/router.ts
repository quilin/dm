import Vue from 'vue';
import Router from 'vue-router';

import GeneralMenu from './views/layout/GeneralMenu.vue';
import GameMenu from './views/pages/game/GameMenu.vue';
import GeneralSidebar from './views/layout/GeneralSidebar.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  routes: [
    {
      name: 'home',
      path: '/',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/home/Home.vue'),
      },
    },
    {
      name: 'error',
      path: '/error/:code',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/error/Error.vue'),
      },
    },

    {
      name: 'activate',
      path: '/activate/:token',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/activate/Activate.vue'),
      }
    },

    {
      path: '/about',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/reviews/Reviews.vue'),
      },
      children: [{
        name: 'about',
        path: ':n?',
        component: () => import('./views/pages/reviews/ReviewsList.vue'),
      }],
    },
    {
      name: 'rules',
      path: '/rules',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/rules/Rules.vue'),
      },
    },
    {
      name: 'donate',
      path: '/donate',
      components: {
        menu: GeneralMenu,
      },
    },
    {
      path: '/community',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/community/Community.vue'),
      },
      children: [{
        name: 'community',
        path: ':n?',
        component: () => import('./views/pages/community/UsersList.vue'),
      }],
    },
    {
      path: '/polls',
      components: {
        menu: GeneralMenu,
        page: () => import('./views/pages/polls/Polls.vue'),
      },
      children: [{
        name: 'polls',
        path: ':n?',
        component: () => import('./views/pages/polls/PollsList.vue'),
      }]
    },
    {
      name: 'chat',
      path: '/fm',
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
        name: 'profile',
        path: '',
        component: () => import('./views/pages/profile/Information.vue'),
      }, {
        name: 'profile-games',
        path: 'games',
      }, {
        name: 'profile-characters',
        path: 'characters',
      }, {
        name: 'profile-settings',
        path: 'settings',
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
        name: 'forum',
        path: ':n?',
        component: () => import('./views/pages/forum/TopicsList.vue'),
      }],
    },
    {
      path: '/topic/:id',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/topic/Topic.vue'),
      },
      children: [{
        name: 'topic',
        path: ':n?',
        component: () => import('./views/pages/topic/TopicComments.vue'),
      }],
    },
    {
      name: 'games',
      path: '/games',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
      },
    },
    {
      name: 'create-game',
      path: '/create-game',
      components: {
        menu: GeneralMenu,
        sidebar: GeneralSidebar,
        page: () => import('./views/pages/create-game/CreateGame.vue'),
      },
    },
    {
      path: '/game/:id',
      components: {
        menu: GameMenu,
        page: () => import('./views/pages/game/Game.vue'),
      },
      children: [{
        name: 'game',
        path: '',
        component: () => import('./views/pages/game/Information.vue'),
      }, {
        name: 'game-comments',
        path: 'out-of-session',
      }, {
        name: 'create-character',
        path: 'create-character',
        component: () => import('./views/pages/game/CreateCharacter.vue'),
      }, {
        name: 'game-characters',
        path: 'characters/:characterId?',
        component: () => import('./views/pages/game/Characters.vue'),
      }, {
        name: 'game-settings',
        path: 'settings',
      }],
    },
    {
      name: 'game-room',
      path: '/session/:id',
    },
  ],
});
