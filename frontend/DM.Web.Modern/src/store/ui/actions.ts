import { ActionTree } from 'vuex';
import UiState from './uiState';
import RootState from './../rootState';
import { ColorSchema } from '@/api/models/community';

const actions: ActionTree<UiState, RootState> = {
  updateTheme({ commit }, { theme }) {
    commit('updateTheme', theme);
  },
  toggleTheme({ commit, state, rootState }) {
    const { theme } = state;
    if (theme !== ColorSchema.Night) {
      commit('updateTheme', ColorSchema.Night);
    } else {
      const userSchema = rootState.user?.settings?.colorSchema ?? ColorSchema.Modern;
      if (userSchema !== ColorSchema.Night) {
        commit('updateTheme', userSchema);
      } else {
        commit('updateTheme', ColorSchema.Modern);
      }
    }
  }
};

export default actions;
