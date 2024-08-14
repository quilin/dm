import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import ForumState from './forumState';
import RootState from './../rootState';

const state: ForumState = {
  news: null,

  fora: [],

  selectedForumId: null,
  moderators: [],
  attachedTopics: null,
  topics: null,

  selectedTopicId: null,
  topic: null,
  comments: null,
};

const fora: Module<ForumState, RootState> = {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};

export default fora;
