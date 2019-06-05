import User from '@/api/models/community/user';

export default interface Comment {
  id: string;
  author: User;
  created: string;
  updated: string | null;
  text: string;
  likes: User[];
}
