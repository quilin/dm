import { MutationTree } from 'vuex';
import UiState from './uiState';
import { ColorSchema } from '@/api/models/community';

const mutations: MutationTree<UiState> = {
  updateTheme(state, payload: ColorSchema) {
    state.theme = payload;
  },
};

export default mutations;
