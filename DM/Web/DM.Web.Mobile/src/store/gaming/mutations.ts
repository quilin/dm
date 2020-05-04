import { MutationTree } from 'vuex';
import GamingState from './gamingState';
import { Game } from '@/api/models/gaming/games';

const mutations: MutationTree<GamingState> = {
  updateOwnGames(state, payload: Game[]) {
    state.ownGames = payload;
  },
};

export default mutations;
