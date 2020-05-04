import { ActionTree } from 'vuex';
import gamingApi from '@/api/requests/gamingApi';
import GamingState from './gamingState';
import RootState from './../rootState';

const actions: ActionTree<GamingState, RootState> = {
  async fetchOwnGames({ commit }): Promise<void> {
    const { data } = await gamingApi.getOwnGames();
    const { resources: games } = data!;
    commit('updateOwnGames', games);
  },
};

export default actions;
