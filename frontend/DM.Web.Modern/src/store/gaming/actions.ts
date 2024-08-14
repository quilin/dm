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
  async createCharacter({ state }, { character, $router }): Promise<void> {
    const { data, error } = await gamingApi.createCharacter(state.selectedGame!.id, character);
    if (error) {
      $router.push({ name: 'error', params: { code: error.code } });
    } else {
      const { resource: character } = data!;
      $router.push({ name: 'game-characters', params: { id: state.selectedGame!.id, characterId: character.id } })
    }
  },

  async selectGame({ commit }, { id, router }): Promise<void> {
    commit('updateSelectedGame', null);

    const { data, error } = await gamingApi.getGame(id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      const { resource: game } = data!;
      commit('updateSelectedGame', game);
    }
  },
  async fetchSelectedGameCharacters({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameCharacters', null);
    const { resources: characters } = await gamingApi.getCharacters(id);
    commit('updateSelectedGameCharacters', characters);
  },
  async fetchSelectedGameRooms({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameRooms', null);
    const { resources: rooms } = await gamingApi.getRooms(id);
    commit('updateSelectedGameRooms', rooms);
  },
  async fetchSelectedGameReaders({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameReaders', null);
    const { resources: readers } = await gamingApi.getReaders(id);
    commit('updateSelectedGameReaders', readers);
  },

  async subscribe({ commit, state }, { router }): Promise<void> {
    const { data, error } = await gamingApi.subscribe(state.selectedGame!.id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code }});
    } else {
      const { resource: reader } = data!;
      commit('addReader', reader);
    }
  },
  async unsubscribe({ commit, state, rootState }, { router }): Promise<void> {
    const { error } = await gamingApi.unsubscribe(state.selectedGame!.id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code }});
    } else {
      commit('removeReader', rootState.user);
    }
  },
};

export default actions;
