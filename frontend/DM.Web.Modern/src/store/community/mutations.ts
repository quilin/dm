import { MutationTree } from 'vuex';
import CommunityState from './communityState';
import { Poll, Review, User } from '@/api/models/community';
import { Envelope, ListEnvelope } from '@/api/models/common';

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
  addPoll(state, payload: Poll) {
    if (state.activePolls !== null) {
      state.activePolls.unshift(payload);
    }
    if (state.polls !== null) {
      state.polls!.resources.unshift(payload);
    }
  },

  updateUsers(state, payload: ListEnvelope<User>) {
    state.users = payload;
  },
  updateSelectedUser(state, payload: { view: User; edit: User }) {
    state.selectedUser = payload;
  },

  updateReviews(state, payload: ListEnvelope<Review>) {
    state.reviews = payload;
  },
  approveReview(state, payload: Envelope<Review>) {
    if (state.reviews === null) return;

    const reviewIndex = state.reviews!.resources.findIndex((r: Review) => r.id === payload.resource.id);
    if (reviewIndex === -1) return;

    state.reviews!.resources.splice(reviewIndex, 1, payload.resource);
  },
  removeReview(state, payload: string) {
    if (state.reviews === null) return;

    const reviewIndex = state.reviews!.resources.findIndex((r: Review) => r.id === payload);
    if (reviewIndex === -1) return;

    state.reviews!.resources.splice(reviewIndex, 1);
  },
};

export default mutations;
