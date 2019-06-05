import User from '@/api/models/community/User';

export default interface RootState {
  theme: string;
  userTheme: string;

  user: User | null;
}
