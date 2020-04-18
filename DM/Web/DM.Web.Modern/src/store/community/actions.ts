import { ActionTree } from 'vuex';
import communityApi from '@/api/requests/communityApi';
import CommunityState from './communityState';
import RootState from './../rootState';

const actions: ActionTree<CommunityState, RootState> = {
  async fetchActivePolls({ commit }): Promise<void> {
    const polls = await communityApi.getPolls(true);
    commit('updateActivePolls', polls.resources);
  },
};

export default actions;
