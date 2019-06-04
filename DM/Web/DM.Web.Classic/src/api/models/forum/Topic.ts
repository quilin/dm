import User from '@/api/models/community/User';
import Forum from '@/api/models/forum/Forum';
import LastComment from '@/api/models/forum/LastComment';

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
