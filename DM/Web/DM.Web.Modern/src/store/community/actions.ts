import { ActionTree } from 'vuex';
import communityApi from '@/api/requests/communityApi';
import CommunityState from './communityState';
import RootState from './../rootState';

const actions: ActionTree<CommunityState, RootState> = {
  async fetchActivePolls({ commit }): Promise<void> {
    const polls = await communityApi.getPolls(true);
    commit('updateActivePolls', polls.resources);
  },
  async vote({ commit }, { router, pollId, optionId }): Promise<void> {
    const { data, error } = await communityApi.postPollVote(pollId, optionId);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('updatePoll', data!.resource);
    }
  },

  async fetchUsers({ commit }, { n }): Promise<void> {
    const users = await communityApi.getUsers(n);
    commit('updateUsers', users);
  },
};

export default actions;
