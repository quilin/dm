import User from '@/api/models/community/user';
import Forum from '@/api/models/forum/forum';
import LastComment from '@/api/models/forum/lastComment';

export default interface Topic {
  id: string;
  author: User;
  created: string;
  title: string;
  description: string;
  attached: boolean;
  closed: boolean;
  lastComment: LastComment | null;
  commentsCount: number;
  unreadCommentsCount: number;
  forum: Forum;
  likes: User[];
}
