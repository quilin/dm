import { ListEnvelope } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';

export default interface ForumState {
  news: Topic[];

  fora: Forum[];

  selectedForumId: string | null;
  moderators: User[];
  attachedTopics: ListEnvelope<Topic> | null;
  topics: ListEnvelope<Topic> | null;

  selectedTopicId: string | null;
  topic: Topic | null;
  comments: ListEnvelope<Comment> | null;
}
