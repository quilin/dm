import Vue from 'vue';
import Vuex from 'vuex';
import RootState from './rootState';
import forum from './forum';

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
  },
  actions: {
    toggleTheme({ commit }): void {
      commit('toggleTheme');
    },
  },
  getters: {
    currentTheme: (state) => state.theme,
  },
  modules: {
    forum,
  },
});
