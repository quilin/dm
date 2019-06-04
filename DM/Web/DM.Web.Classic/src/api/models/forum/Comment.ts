import User from '@/api/models/community/User';

export default interface Comment {
  id: string;
  author: User;
  created: string;
  updated: string | null;
  text: string;
  likes: User[];
}
