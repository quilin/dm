import { GetterTree } from 'vuex';
import CommunityState from './communityState';
import RootState from './../rootState';
import { Poll } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

const getters: GetterTree<CommunityState, RootState> = {
  activePolls(state): Poll[] | null {
    return state.activePolls;
  },
  polls(state): ListEnvelope<Poll> | null {
    return state.polls;
  }
};

export default getters;
