import User from '@/api/models/community/User';

export default interface LastComment {
  created: string;
  author: User;
}
