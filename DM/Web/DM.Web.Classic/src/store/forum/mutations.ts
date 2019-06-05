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
  updateTopics(state, payload: {attachedTopics: ListEnvelope<Topic>, topics: ListEnvelope<Topic>}) {
    state.attachedTopics = payload.attachedTopics;
    state.topics = payload.topics;
  },
};

export default mutations;
