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
  async fetchTopics({ commit }, { id, page }): Promise<void> {
    const data = await forumApi.getTopics(id, page);
    commit('updateTopics', data);
  },
};

export default actions;
