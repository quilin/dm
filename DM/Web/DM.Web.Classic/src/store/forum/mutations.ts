import { MutationTree } from 'vuex';
import ForumState from './forumState';

import ListEnvelope from '@/api/models/common/listEnvelope';
import User from '@/api/models/community/user';
import { Forum, Topic } from '@/api/models/forum';

const mutations: MutationTree<ForumState> = {
  updateFora(state, payload: Forum[]) {
    state.fora = payload;
  },
  updateNews(state, payload: Topic[]) {
    state.news = payload;
  },

  updateSelectedForum(state, payload: string) {
    state.selectedForumId = payload;
  },
  updateModerators(state, payload: User[]) {
    state.moderators = payload;
  },
  updateTopics(state, payload: ListEnvelope<Topic>) {
    state.topics = payload;
  },
};

export default mutations;
