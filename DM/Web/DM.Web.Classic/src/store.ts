import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
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
});
