import { User } from '@/api/models/community';

export default interface Likeable
{
  likes: User[];
  author: User;
}