import { GetterTree } from 'vuex';
import ForumState from './forumState';
import RootState from './../rootState';
import User from '@/api/models/community/user';
import { Forum, Topic } from '@/api/models/forum';

const getters: GetterTree<ForumState, RootState> = {
  fora(state): Forum[] {
    return state.fora;
  },
  news(state): Topic[] {
    return state.news;
  },
  moderators(state): User[] {
    return state.moderators;
  },
  attachedTopics(state): Topic[] | null {
    if (state.attachedTopics === null) {
      return null;
    }
    return state.attachedTopics!.resources;
  },
  topics(state): Topic[] | null {
    if (state.topics === null) {
      return null;
    }
    return state.topics!.resources;
  },
  selectedForum(state): string | null {
    return state.selectedForumId;
  },
};

export default getters;
