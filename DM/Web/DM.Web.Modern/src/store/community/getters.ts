import { GetterTree } from 'vuex';
import CommunityState from './communityState';
import RootState from './../rootState';
import { Poll, Review, User } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

const getters: GetterTree<CommunityState, RootState> = {
  activePolls(state): Poll[] | null {
    return state.activePolls;
  },
  polls(state): ListEnvelope<Poll> | null {
    return state.polls;
  },

  users(state): ListEnvelope<User> | null {
    return state.users;
  },
  selectedUser(state): User | null {
    return state.selectedUser?.view ?? null;
  },
  editableUser(state): User | null {
    return state.selectedUser?.edit ?? null;
  },

  reviews(state): ListEnvelope<Review> | null {
    return state.reviews;
  }
};

export default getters;
