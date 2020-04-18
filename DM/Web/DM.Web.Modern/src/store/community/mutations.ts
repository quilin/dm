import { MutationTree } from 'vuex';
import CommunityState from './communityState';
import { Poll } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

const mutations: MutationTree<CommunityState> = {
  updateActivePolls(state, payload: Poll[]) {
    state.activePolls = payload;
  },
  updatePolls(state, payload: ListEnvelope<Poll>) {
    state.polls = payload;
  }
};

export default mutations;
