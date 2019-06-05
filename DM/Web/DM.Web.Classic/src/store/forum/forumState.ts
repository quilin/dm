import Forum from '@/api/models/forum/Forum';
import Topic from '@/api/models/forum/Topic';

export default interface ForumState {
  fora: Forum[];
  selectedForumId: string | null;

  news: Topic[];
}
