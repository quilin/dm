import { GetterTree } from 'vuex';
import ForumState from './forumState';
import RootState from './../rootState';
import { ListEnvelope, Paging } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';

const getters: GetterTree<ForumState, RootState> = {
  fora(state): Forum[] {
    return state.fora;
  },
  news(state): Topic[] | null {
    return state.news;
  },

  selectedForum(state): string | null {
    return state.selectedForumId;
  },
  selectedTopic(state): string | null {
    return state.selectedTopicId;
  },
  forum(state): Forum | null {
    return state.fora.find((f: Forum) => f.id === state.selectedForumId) || null;
  },
  moderators(state): User[] {
    return state.moderators;
  },
  attachedTopics(state): Topic[] | null {
    return state.attachedTopics;
  },
  topics(state): Topic[] | null {
    return state.topics?.resources || null;
  },
  topicsPaging(state): Paging | null {
    return state.topics?.paging || null;
  },
  topic(state): Topic | null {
    return state.topic || state.topics?.resources.find((t: Topic) => t.id === state.selectedTopicId) || null;
  },
  comments(state): ListEnvelope<Comment> | null {
    return state.comments;
  },
};

export default getters;
