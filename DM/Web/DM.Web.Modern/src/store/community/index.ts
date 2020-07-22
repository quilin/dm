import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import CommunityState from './communityState';
import RootState from './../rootState';

const state: CommunityState = {
  activePolls: null,
  polls: null,

  users: null,
  selectedUser: null,

  reviews: null,
};

const community: Module<CommunityState, RootState> = {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};

export default community;
