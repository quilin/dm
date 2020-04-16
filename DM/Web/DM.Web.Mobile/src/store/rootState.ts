import { User } from '@/api/models/community';

export default interface RootState {
  theme: string;
  userTheme: string;

  user: User | null;
  unreadConversations: number;
}
