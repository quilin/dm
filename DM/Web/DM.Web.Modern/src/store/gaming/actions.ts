import { ActionTree } from 'vuex';
import gamingApi from '@/api/requests/gamingApi';
import GamingState from './gamingState';
import RootState from './../rootState';
import { AttributeSchema } from '@/api/models/gaming';

const actions: ActionTree<GamingState, RootState> = {
  async fetchOwnGames({ commit }): Promise<void> {
    const { resources: games } = await gamingApi.getOwnGames();
    commit('updateOwnGames', games);
  },

  async fetchPopularGames({commit}): Promise<void> {
    const { resources: games } = await gamingApi.getPopularGames();
    commit('updatePopularGames', games);
  },

  async fetchSchemas({ commit }): Promise<void> {
    const { resources: schemas } = await gamingApi.getSchemas();
    commit('updateSchemas', schemas);
  },
  async fetchTags({ commit }): Promise<void> {
    const { resources: tags } = await gamingApi.getTags();
    commit('updateTags', tags);
  },

  async createSchema({ commit }, { schema, $router }): Promise<AttributeSchema> {
    const { data, error } = await gamingApi.createSchema(schema);
    if (error) {
      $router.push({ name: 'error', params: { code: error.code } });
      return Promise.reject();
    } else {
      const { resource: newSchema } = data!;
      commit('addSchema', newSchema);
      return newSchema;
    }
  },
  async createGame(_, { game, $router }): Promise<void> {
    const { data, error } = await gamingApi.createGame(game);
    if (error) {
      $router.push({ name: 'error', params: { code: error.code } });
    } else {
      const { resource: game } = data!;
      $router.push({ name: 'game', params: { id: game.id } });
    }
  },
};

export default actions;
