import User from '@/api/models/community/user';

export default interface LastComment {
  created: string;
  author: User;
}
