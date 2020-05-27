import { MutationTree } from 'vuex';
import ForumState from './forumState';

import { ListEnvelope } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';

const mutations: MutationTree<ForumState> = {
  updateFora(state, payload: Forum[]) {
    state.fora = payload;
  },
  updateNews(state, payload: ListEnvelope<Topic>) {
    state.news = payload.resources;
  },
  updateSelectedForum(state, payload: string) {
    state.selectedForumId = payload;
  },
  updateModerators(state, payload: User[]) {
    state.moderators = payload;
  },
  updateTopics(state, payload: { attachedTopics: ListEnvelope<Topic>; topics: ListEnvelope<Topic> }) {
    state.attachedTopics = payload.attachedTopics;
    state.topics = payload.topics;
  },
  clearTopics(state) {
    state.attachedTopics = null;
    if (state.topics) {
      state.topics!.resources = [];
    }
  },
  updateSelectedTopic(state, payload: { topic: Topic; id: string }) {
    state.selectedTopicId = payload.id;
    state.topic = payload.topic;
  },
  updateComments(state, payload: ListEnvelope<Comment>) {
    state.comments = payload;
  },
  markAllTopicsAsRead(state) {
    state.fora.forEach(f => {
      if (f.id === state.selectedForumId) {
        f.unreadTopicsCount = 0;
      }
    });

    state.topics!.resources.forEach(topic => {
      topic.unreadCommentsCount = 0;
    });
  }
};

export default mutations;
