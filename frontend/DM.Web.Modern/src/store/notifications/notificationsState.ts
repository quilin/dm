import { UserNotification } from '@/api/models/notifications';

export default interface NotificationsState {
  notifications: UserNotification[];
}