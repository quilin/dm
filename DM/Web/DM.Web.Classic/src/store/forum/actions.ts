import { ActionTree } from 'vuex';
import forumApi from '@/api/requests/forumApi';
import ForumState from './forumState';
import RootState from './../rootState';

const actions: ActionTree<ForumState, RootState> = {
  async fetchFora({ commit }): Promise<void> {
    const { resources } = await forumApi.get();
    commit('updateFora', resources);
  },
  async fetchNews({ commit }): Promise<void> {
    const { resources } = await forumApi.getNews();
    commit('updateNews', resources);
  },

  async fetchModerators({ commit }, id): Promise<void> {
    commit('updateModerators', []);
    const { resources } = await forumApi.getModerators(id);
    commit('updateModerators', resources);
  },
  async fetchTopics({ commit }, { id, n }): Promise<void> {
    commit('updateTopics', { attachedTopics: null, topics: null });
    const [attachedTopics, topics] = await Promise.all([
    forumApi.getTopics(id, true, 1),
    forumApi.getTopics(id, false, n)]);
    commit('updateTopics', { attachedTopics, topics });
  },
  async selectForum({ commit }, id): Promise<void> {
    commit('updateSelectedForum', id);
  },

  async selectTopic({ commit }, id): Promise<void> {
    commit('updateSelectedTopic', { topic: null, id });
    const { resource } = await forumApi.getTopic(id);
    commit('updateSelectedTopic', { topic: resource, id });
    commit('updateSelectedForum', resource.forum.id);
  },
  async fetchComments({ commit }, { id, n }): Promise<void> {
    commit('updateComments', null);
    const comments = await forumApi.getComments(id, n);
    commit('updateComments', comments);
  },
};

export default actions;
