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
    authenticate(state, user): void {
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
      commit('authenticate', user);
    },
    async signOut({ commit }): Promise<void> {
      await accountApi.signOut();
      commit('authenticate', null);
    },
    fetchUser({ commit }): void {
      const serializedUser = localStorage.getItem('user');
      if (serializedUser) {
        commit('authenticate', JSON.parse(serializedUser));
      }
    },
  },
  getters: {
    currentTheme: (state) => state.theme,
    user: (state) => state.user,
  },
  modules: {
    forum,
  },
});
