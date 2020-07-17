import Vue from 'vue';
import Vuex from 'vuex';
import RootState from './rootState';
import forum from './forum';
import community from './community';
import gaming from './gaming';
import ui from './ui';
import accountApi from '@/api/requests/accountApi';
import { ColorSchema } from '@/api/models/community';

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
    authenticate({ commit }, user): void {
      commit('updateUser', user);
      commit('ui/updateTheme', user?.settings?.colorSchema ?? ColorSchema.Modern);
    },
    async signOut({ commit }): Promise<void> {
      await accountApi.signOut();
      commit('updateUser', null);
    },
    async fetchUser({ commit }): Promise<void> {
      const serializedUser = localStorage.getItem('user');
      if (serializedUser) {
        commit('updateUser', JSON.parse(serializedUser));
        const { data } = await accountApi.fetchUser();
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
  },
});
