import { GetterTree } from 'vuex';
import ForumState from './forumState';
import RootState from './../rootState';
import { Paging } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';

const getters: GetterTree<ForumState, RootState> = {
  fora(state): Forum[] {
    return state.fora;
  },
  news(state): Topic[] {
    return state.news;
  },

  selectedForum(state): string | null {
    return state.selectedForumId;
  },
  selectedTopic(state): string | null {
    return state.selectedTopicId;
  },

  moderators(state): User[] {
    return state.moderators;
  },
  attachedTopics(state): Topic[] | null {
    return state.attachedTopics && state.attachedTopics!.resources;
  },
  topics(state): Topic[] | null {
    return state.topics && state.topics!.resources;
  },
  topicsPaging(state): Paging | null {
    return state.topics && state.topics!.paging;
  },

  topic(state): Topic | null {
    return state.topic ||
      state.topics &&
      state.topics!.resources &&
      (state.topics!.resources!.find((t: Topic) => t.id === state.selectedTopicId) || null);
  },
  comments(state): Comment[] | null {
    return state.comments && state.comments!.resources;
  },
  commentsPaging(state): Paging | null {
    return state.comments && state.comments!.paging;
  },
};

export default getters;
