import { GetterTree } from 'vuex';
import UiState from './uiState';
import RootState from './../rootState';

const getters: GetterTree<UiState, RootState> = {
  theme(state): string {
    return state.theme.toLowerCase();
  },
};

export default getters;
