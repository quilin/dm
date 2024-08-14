import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import UiState from './uiState';
import RootState from './../rootState';
import { ColorSchema } from '@/api/models/community';

const state: UiState = {
  theme: ColorSchema.Modern,
};

const gaming: Module<UiState, RootState> = {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};

export default gaming;
