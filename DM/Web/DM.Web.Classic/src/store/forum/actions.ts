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
  async selectForum({ commit }, { id }): Promise<void> {
    commit('updateSelectedForum', id);
    const { resources } = await forumApi.getModerators(id);
    commit('updateModerators', resources);
  },
  async fetchTopics({ commit }, { id, n }): Promise<void> {
    const [attachedTopics, topics] = await Promise.all([
      forumApi.getTopics(id, true, 1),
      forumApi.getTopics(id, false, n)]);
    commit('updateTopics', { attachedTopics, topics });
  },
};

export default actions;
