import { ActionTree } from 'vuex';
import NotificationsState from '@/store/notifications/notificationsState';
import RootState from '@/store/rootState';
import { UserNotification } from '@/api/models/notifications';

const actions: ActionTree<NotificationsState, RootState> = {
  push({ commit }, data: UserNotification) {
    commit('push', data);
  }
};

export default actions;
