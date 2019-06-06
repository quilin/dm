import ListEnvelope from '@/api/models/common/listEnvelope';
import User from '@/api/models/community/user';
import { Forum, Topic } from '@/api/models/forum';

export default interface ForumState {
  news: Topic[];

  fora: Forum[];
  selectedForumId: string | null;

  moderators: User[];
  attachedTopics: ListEnvelope<Topic> | null;
  topics: ListEnvelope<Topic> | null;

  topic: Topic;
}
