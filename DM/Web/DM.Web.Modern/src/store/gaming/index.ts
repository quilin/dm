import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import GamingState from './gamingState';
import RootState from './../rootState';

const state: GamingState = {
  ownGames: null,
  popularGames: null,
  schemas: null,
  tags: null,

  selectedGame: null,
  selectedGameCharacters: null,
  selectedGameRooms: null,
  selectedGameReaders: null,
};

const gaming: Module<GamingState, RootState> = {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};

export default gaming;
