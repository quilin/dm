import Vue from 'vue';
import Vuex from 'vuex';
import RootState from './rootState';
import forum from './forum';
import community from './community';
import gaming from './gaming';
import notifications from './notifications';
import ui from './ui';
import accountApi from '@/api/requests/accountApi';
import { ColorSchema, LoginCredentials, User } from '@/api/models/community';

Vue.use(Vuex);

export default new Vuex.Store<RootState>({
  state: {
    user: null,
    unreadConversations: 0,
  },
  mutations: {
    updateUser(state, user): void {
      state.user = user;
      if (user === null) {
        localStorage.removeItem('user');
      } else {
        localStorage.setItem('user', JSON.stringify(user));
      }
    },
  },
  actions: {
    async authenticate({ commit }, credentials: LoginCredentials): Promise<void> {
      const { data, error } = await accountApi.signIn(credentials);
      if (error) {
        // todo
        return;
      }

      const { resource: user } = data!;
      commit('updateUser', user);
      commit('ui/updateTheme', user?.settings?.colorSchema ?? ColorSchema.Modern);
    },
    async signOut({ commit }): Promise<void> {
      await accountApi.signOut();
      commit('updateUser', null);
    },
    async fetchUser({ commit }): Promise<void> {
      if (!accountApi.isAuthenticated()) {
        return;
      }

      const serializedUser = localStorage.getItem('user');
      if (serializedUser) {
        console.info('Fetching cached user, but still requesting it from server');
        const storedUser = JSON.parse(serializedUser) as User;
        commit('updateUser', storedUser);
        commit('ui/updateTheme', storedUser.settings?.colorSchema ?? ColorSchema.Modern);
      }

      const { data, error } = await accountApi.fetchUser();
      if (error) {
        commit('updateUser', null);
        commit('ui/updateTheme', ColorSchema.Modern);
      } else {
        const user = data!.resource;
        commit('updateUser', data!.resource);
        commit('ui/updateTheme', user.settings?.colorSchema ?? ColorSchema.Modern);
      }
    },
  },
  getters: {
    user: (state) => state.user,
    unreadConversations: (state) => state.unreadConversations,
  },
  modules: {
    ui,
    forum,
    community,
    gaming,
    notifications,
  },
});
