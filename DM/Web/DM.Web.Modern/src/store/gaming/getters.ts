import { GetterTree } from 'vuex';
import GamingState from './gamingState';
import RootState from './../rootState';
import { Game } from '@/api/models/gaming/games';

const getters: GetterTree<GamingState, RootState> = {
  ownGames(state): Game[] | null {
    return state.ownGames;
  },
};

export default getters;
