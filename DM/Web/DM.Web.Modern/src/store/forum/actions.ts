import { ActionTree } from 'vuex';
import forumApi from '@/api/requests/forumApi';
import { Topic } from '@/api/models/forum';
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
      router.push({ name: 'error', params: { code: 404 } });
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
    commit('updateComments', null);
    const { data, error } = await forumApi.getComments(id, n);
    if (!error) {
      commit('updateComments', data!);
    }
  },
  async markAllTopicsAsRead({ commit }, { id }): Promise<void> {
    const { error } = await forumApi.markAllTopicsAsRead(id);
    if (!error) {
      commit('markAllTopicsAsRead');
    }
  },
};

export default actions;
