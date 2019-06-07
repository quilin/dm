import Vue from 'vue';
import Vuex from 'vuex';
import RootState from './rootState';
import forum from './forum';
import accountApi from '@/api/requests/accountApi';

Vue.use(Vuex);

export default new Vuex.Store<RootState>({
  state: {
    theme: 'modern',
    userTheme: 'modern',

    user: null,
    unreadConversations: 0,
  },
  mutations: {
    toggleTheme(state): void {
      if (state.theme === 'night') {
        state.theme = state.userTheme;
      } else {
        state.userTheme = state.theme;
        state.theme = 'night';
      }
    },
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
    toggleTheme({ commit }): void {
      commit('toggleTheme');
    },
    authenticate({ commit }, user): void {
      commit('updateUser', user);
    },
    async signOut({ commit }): Promise<void> {
      await accountApi.signOut();
        commit('updateUser', null);
    },
    async fetchUser({ commit }): Promise<void> {
      const serializedUser = localStorage.getItem('user');
      if (serializedUser) {
        commit('updateUser', JSON.parse(serializedUser));
        const { resource } = await accountApi.fetchUser();
        commit('updateUser', resource);
      }
    },
  },
  getters: {
    currentTheme: (state) => state.theme,
    user: (state) => state.user,
    unreadConversations: (state) => state.unreadConversations,
  },
  modules: {
    forum,
  },
});
