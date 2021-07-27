import { ActionTree } from 'vuex';
import forumApi from '@/api/requests/forumApi';
import { Comment, Topic } from '@/api/models/forum';
import ForumState from './forumState';
import RootState from './../rootState';
import { PagingQuery } from '@/api/models/common';

const actions: ActionTree<ForumState, RootState> = {
  async fetchFora({ commit }): Promise<void> {
    const { data } = await forumApi.getFora();
    const { resources } = data!;
    commit('updateFora', resources);
  },
  async fetchNews({ commit }): Promise<void> {
    const { data } = await forumApi.getNews();
    commit('updateNews', data!);
  },

  async fetchModerators({ commit }, { id }): Promise<void> {
    commit('updateModerators', []);
    const { data, error } = await forumApi.getModerators(id);
    if (!error) {
      commit('updateModerators', data!.resources);
    }
  },
  async fetchTopics({ commit }, { id, n }): Promise<void> {
    commit('clearTopics');
    const [attachedTopics, topics] = await Promise.all([
      forumApi.getTopics(id, { number: 1 } as PagingQuery, true),
      forumApi.getTopics(id, { number: n } as PagingQuery, false)]);
    commit('updateTopics', {
      attachedTopics: attachedTopics.data!.resources,
      topics: topics.data,
    });
  },
  async selectForum({ commit }, { id, router }): Promise<void> {
    commit('updateSelectedForum', id);
    const { error } = await forumApi.getForum(id);
    if (error !== null) {
      router.push({ name: 'error', params: { code: error.code } });
    }
  },
  async createTopic(_0, { router, topic }): Promise<void> {
    const { data, error } = await forumApi.postTopic(topic as Topic);

    if (data && !error) {
      router.push({ name: 'topic', params: { id: data.resource.id } });
    }
  },
  async selectTopic({ commit }, { id }): Promise<void> {
    commit('updateSelectedTopic', { topic: null, id });
    const { error, data } = await forumApi.getTopic(id);
    if (!error) {
      const resource = data!.resource;
      commit('updateSelectedTopic', { topic: resource, id });
      commit('updateSelectedForum', resource.forum.id);
    }
  },
  async fetchComments({ commit }, { id, n }): Promise<void> {
    const { data, error } = await forumApi.getComments(id, { number: n } as PagingQuery);

    if (!error) {
      commit('updateComments', data!);
    }
  },
  async createComment({ commit }, { id, router, comment }): Promise<void> {
    const { error: postCommentError } = await forumApi.postComment(id, comment as Comment);

    if (postCommentError) return Promise.reject();

    const { data: commentsData, error: commentsError } =
      await forumApi.getComments(id, { size: 0, number: 0, skip: 0 });

    if (!commentsError && commentsData?.paging) {
      commit('updateComments', commentsData);
      router.push({ name: 'topic', params: { id, n: commentsData.paging.total } });
    }
  },
  async updateComment({ commit }, { id, comment }): Promise<void> {
    const { data, error } = await forumApi.updateComment(id, comment as Comment);

    if (!error) {
      commit('updateComment', data?.resource);
    }
  },
  async deleteComment(_0, { id }): Promise<void> {
    await forumApi.deleteComment(id);
  },
  async markAllTopicsAsRead({ commit }, { id }): Promise<void> {
    const { error } = await forumApi.markAllTopicsAsRead(id);
    if (!error) {
      commit('markAllTopicsAsRead');
    }
  },
  async markTopicAsRead({ commit }, { id }): Promise<void> {
    const { error } = await forumApi.markTopicAsRead(id);
    if (!error) {
      commit('markTopicAsRead');
    }
  },
  async addCommentLike({ commit, rootState }, { id }): Promise<void> {
    commit('addCommentLike', { id, user: rootState.user });
    await forumApi.postCommentLike(id);
  },
  async deleteCommentLike({ commit, rootState }, { id }): Promise<void> {
    commit('deleteCommentLike', { id, user: rootState.user });
    await forumApi.deleteCommentLike(id);
  },
  async addTopicLike({ commit, rootState }, { id }): Promise<void> {
    commit('addTopicLike', { user: rootState.user });
    await forumApi.postTopicLike(id);
  },
  async deleteTopicLike({ commit, rootState }, { id }): Promise<void> {
    commit('deleteTopicLike', { user: rootState.user });
    await forumApi.deleteTopicLike(id);
  },

};

export default actions;
