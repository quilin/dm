import { User } from '@/api/models/community';

export default interface RootState {
  user: User | null;
  unreadConversations: number;
}
