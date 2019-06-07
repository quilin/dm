import { MutationTree } from 'vuex';
import ForumState from './forumState';

import { ListEnvelope } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';

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
  clearTopics(state) {
    state.attachedTopics = null;
    if (state.topics) {
      state.topics!.resources = null;
    }
  },

  updateSelectedTopic(state, payload: {topic: Topic, id: string}) {
    state.selectedTopicId = payload.id;
    state.topic = payload.topic;
  },
  updateComments(state, payload: ListEnvelope<Comment>) {
    state.comments = payload;
  },
};

export default mutations;
