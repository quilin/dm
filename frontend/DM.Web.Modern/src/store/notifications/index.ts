import { Module } from 'vuex';
import getters from './getters';
import actions from './actions';
import mutations from './mutations';
import NotificationsState from './notificationsState';
import RootState from './../rootState';

const state: NotificationsState = {
  notifications: [],
};

const notifications: Module<NotificationsState, RootState> = {
  namespaced: true,
  state,
  actions,
  getters,
  mutations,
};

export default notifications;
