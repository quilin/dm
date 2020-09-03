import { GetterTree } from 'vuex';
import GamingState from './gamingState';
import RootState from './../rootState';

const getters: GetterTree<GamingState, RootState> = {
  ownGames: (state) => state.ownGames,
  schemas: (state) => state.schemas,
  tags: (state) => state.tags,
};

export default getters;
