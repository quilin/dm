import { ActionTree, Commit } from 'vuex';
import communityApi from '@/api/requests/communityApi';
import CommunityState from './communityState';
import RootState from './../rootState';
import { User, Review } from '@/api/models/community';
import { PagingQuery } from '@/api/models/common';
import { userIsAdmin } from '@/api/models/community/helpers';

async function updateUserPart(commit: Commit, state: CommunityState, router: any, userPart: User): Promise<void> {
  if (state.selectedUser === null) return;
  const user = state.selectedUser!.edit;
  const { data, error } = await communityApi.updateUser(user.login, userPart);
  if (error) {
    router.push({ name: 'error', params: { code: error.code } });
  } else {
    commit('updateSelectedUser', { ...state.selectedUser, view: data!.resource });
  }
}

const actions: ActionTree<CommunityState, RootState> = {
  async fetchActivePolls({ commit }): Promise<void> {
    const { resources } = await communityApi.getPolls({ size: 10 } as PagingQuery, true);
    commit('updateActivePolls', resources);
  },
  async vote({ commit }, { router, pollId, optionId }): Promise<void> {
    const { data, error } = await communityApi.postPollVote(pollId, optionId);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('updatePoll', data!.resource);
    }
  },
  async fetchPolls({ commit }, { n }): Promise<void> {
    const polls = await communityApi.getPolls({ number: n } as PagingQuery, false);
    commit('updatePolls', polls);
  },
  async createPoll({ commit }, { router, poll }): Promise<void> {
    const { data, error } = await communityApi.postPoll(poll);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('addPoll', data!.resource);
    }
  },

  async fetchUsers({ commit }, query): Promise<void> {
    const users = await communityApi.getUsers(query);
    commit('updateUsers', users);
  },
  async selectUser({ commit }, { login, router }): Promise<void> {
    const { data, error } = await communityApi.getUser(login);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      const viewUser = data!.resource;
      const editUser = await communityApi.getUserForUpdate(login);
      commit('updateSelectedUser', { view: viewUser, edit: editUser });
    }
  },

  async updateInformation({ commit, state }, { router }): Promise<void> {
    const user = state.selectedUser!.edit;
    await updateUserPart(commit, state, router, { info: user.info } as User);
  },
  async updateSettings({ commit, state }, { router }): Promise<void> {
    const user = state.selectedUser!.edit;
    await updateUserPart(commit, state, router, { settings: user.settings } as User);
  },
  async uploadProfilePicture({ commit, state, rootState }, { file, progressCallback }): Promise<void> {
    const user = state.selectedUser!.edit;
    const { resource } = await communityApi.uploadUserPicture(user.login, file, progressCallback);
    commit('updateSelectedUser', { ...state.selectedUser, view: resource });

    if (rootState.user!.login === user.login) {
      commit('updateUser', resource, { root: true });
    }
  },

  async fetchReviews({ commit, rootState }, { n }): Promise<void> {
    const canApprove = userIsAdmin(rootState.user);
    const reviews = await communityApi.getReviews({ number: n } as PagingQuery, !canApprove);
    commit('updateReviews', reviews);
  },
  async approveReview({ commit, dispatch }, { id, router, route }): Promise<void> {
    const { data, error } = await communityApi.updateReview(id, { approved: true } as Review);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('approveReview', data);
      dispatch('fetchReviews', route.params);
    }
  },
  async removeReview({ commit, dispatch }, { id, router, route }): Promise<void> {
    const { error } = await communityApi.removeReview(id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('removeReview', id);
      dispatch('fetchReviews', route.params);
    }
  },
};

export default actions;
