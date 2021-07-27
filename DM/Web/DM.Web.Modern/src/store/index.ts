import Vue from 'vue';
import Vuex from 'vuex';
import RootState from './rootState';
import forum from './forum';
import community from './community';
import gaming from './gaming';
import notifications from './notifications';
import ui from './ui';
import accountApi from '@/api/requests/accountApi';
import { ColorSchema, LoginCredentials } from '@/api/models/community';

Vue.use(Vuex);

export default new Vuex.Store<RootState>({
  state: {
    user: null,
    unreadConversations: 0,
  },
  mutations: {
    updateUser(state, user): void {
      state.user = user;
      if (user) {
        localStorage.setItem('user', JSON.stringify(user));
      } else {
        localStorage.removeItem('user');
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
      const serializedUser = localStorage.getItem('user');
      if (!serializedUser) return;

      accountApi.restoreUser();
      commit('updateUser', JSON.parse(serializedUser));
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
