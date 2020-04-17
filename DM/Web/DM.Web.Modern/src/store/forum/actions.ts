import { ActionTree } from 'vuex';
import forumApi from '@/api/requests/forumApi';
import { Topic } from '@/api/models/forum';
import ForumState from './forumState';
import RootState from './../rootState';

const actions: ActionTree<ForumState, RootState> = {
  async fetchFora({ commit }): Promise<void> {
    const { resources } = await forumApi.getFora();
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
      forumApi.getTopics(id, true, 1),
      forumApi.getTopics(id, false, n)]);
    commit('updateTopics', {
      attachedTopics: attachedTopics.data,
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
  async createTopic({ commit }, { title, description }): Promise<void> {
    const { error, data } = await forumApi.postTopic({title, description} as Topic);
    if (!error) {
      console.log('created!');
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
};

export default actions;
