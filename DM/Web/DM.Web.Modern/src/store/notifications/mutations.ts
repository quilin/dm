import { MutationTree } from 'vuex';
import NotificationsState from '@/store/notifications/notificationsState';
import { UserNotification } from '@/api/models/notifications';

const mutations: MutationTree<NotificationsState> = {
  push(state, payload: UserNotification) {
    state.notifications.push(payload);
  }
};

export default mutations;
