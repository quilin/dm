import { ActionTree, Commit } from 'vuex';
import communityApi from '@/api/requests/communityApi';
import CommunityState from './communityState';
import RootState from './../rootState';
import { User, UserRole, Review } from '@/api/models/community';
import { PagingQuery } from '@/api/models/common';

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

  async fetchUsers({ commit }, { n }): Promise<void> {
    const users = await communityApi.getUsers(n);
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

  async fetchReviews({ commit, rootState }, { n }): Promise<void> {
    const canApprove = rootState.user !== null &&
      rootState.user.roles.some((r: UserRole) => r === UserRole.Administrator);
    const reviews = await communityApi.getReviews({ number: n, size: 2 } as PagingQuery, !canApprove);
    commit('updateReviews', reviews);
  },
  async approveReview({ commit }, { id, router }): Promise<void> {
    const { data, error } = await communityApi.updateReview(id, { approved: true } as Review);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('approveReview', data);
    }
  },
  async removeReview({ commit }, { id, router }): Promise<void> {
    const { error } = await communityApi.removeReview(id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('removeReview', id);
    }
  },
};

export default actions;
