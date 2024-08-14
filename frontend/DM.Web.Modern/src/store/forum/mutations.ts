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
  updateTopics(state, payload: { attachedTopics: Topic[]; topics: ListEnvelope<Topic> }) {
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
  updateComment(state, payload: Comment) {
    state.comments!.resources = state.comments!.resources.map(comment => {
      if (comment.id === payload.id) {
        return payload;
      }

      return comment;
    });
  },
  markAllTopicsAsRead(state) {
    state.fora.forEach((f: Forum) => {
      if (f.id === state.selectedForumId) {
        f.unreadTopicsCount = 0;
      }
    });

    state.topics!.resources.forEach((topic: Topic) => {
      topic.unreadCommentsCount = 0;
    });
  },
  markTopicAsRead(state) {
    state.fora.forEach((f: Forum) => {
      if (f.id === state.selectedForumId) {
        f.unreadTopicsCount = f.unreadTopicsCount - 1;
      }
    });

    state.topics!.resources.forEach((topic: Topic) => {
      if (topic.id === state.selectedTopicId) {
        topic.unreadCommentsCount = 0;
      }
    });

    state.topic!.unreadCommentsCount = 0;
  },

  addCommentLike(state, payload: { user: User; id: string}) {
    const { user, id } = payload;
    const comment = state.comments!.resources.find(comment => comment.id === id);
    comment!.likes.push(user);
  },
  deleteCommentLike(state, payload: { user: User; id: string }) {
    const { user, id } = payload;
    const comment = state.comments!.resources.find(comment => comment.id === id);
    const likedIndex = comment!.likes.findIndex(likedUser => likedUser.login === user.login);
    if (likedIndex >= 0) {
      comment!.likes.splice(likedIndex, 1);
    }
  },

  addTopicLike(state, payload: { user: User }) {
    const { user } = payload;
    state.topic!.likes!.push(user);
  },
  deleteTopicLike(state, payload: { user: User }) {
    const { user } = payload;
    const likes = state.topic!.likes!;
    const likedIndex = likes.findIndex(likedUser => likedUser.login === user.login);
    if (likedIndex >= 0) {
      likes.splice(likedIndex, 1);
    }
  },
};

export default mutations;
