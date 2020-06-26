import { MutationTree } from 'vuex';
import CommunityState from './communityState';
import { Poll, User } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

const mutations: MutationTree<CommunityState> = {
  updateActivePolls(state, payload: Poll[]) {
    state.activePolls = payload;
  },
  updatePolls(state, payload: ListEnvelope<Poll>) {
    state.polls = payload;
  },
  updatePoll(state, payload: Poll) {
    if (state.activePolls !== null) {
      const matchingActivePoll = state.activePolls.find((poll: Poll) => poll.id === payload.id);
      Object.assign(matchingActivePoll, payload);
    }
    if (state.polls !== null) {
      const matchingPoll = state.polls!.resources.find((poll: Poll) => poll.id === payload.id);
      Object.assign(matchingPoll, payload);
    }
  },

  updateUsers(state, payload: ListEnvelope<User>) {
    state.users = payload;
  },
};

export default mutations;
