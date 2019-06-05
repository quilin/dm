import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import ForumState from './forumState';
import RootState from './../rootState';

const state: ForumState = {
  fora: [],
  selectedForumId: null,
  news: [],
  moderators: [],
  topics: {paging: null, resources: []},
};

const fora: Module<ForumState, RootState> = {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};

export default fora;
