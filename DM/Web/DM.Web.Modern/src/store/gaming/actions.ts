import { ActionTree } from 'vuex';
import gamingApi from '@/api/requests/gamingApi';
import GamingState from './gamingState';
import RootState from './../rootState';
import {AttributeSchema, Comment} from '@/api/models/gaming';
import { PagingQuery } from '@/api/models/common';

const actions: ActionTree<GamingState, RootState> = {
  async fetchOwnGames({ commit }): Promise<void> {
    const { resources: games } = await gamingApi.getOwnGames();
    commit('updateOwnGames', games);
  },

  async fetchPopularGames({ commit }): Promise<void> {
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

  async fetchSelectedGameReaders({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameReaders', null);
    const { resources: readers } = await gamingApi.getReaders(id);
    commit('updateSelectedGameReaders', readers);
  },
  async subscribe({ commit, state }, { router }): Promise<void> {
    const { data, error } = await gamingApi.subscribe(state.selectedGame!.id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      const { resource: reader } = data!;
      commit('addReader', reader);
    }
  },
  async unsubscribe({ commit, state, rootState }, { router }): Promise<void> {
    const { error } = await gamingApi.unsubscribe(state.selectedGame!.id);
    if (error) {
      router.push({ name: 'error', params: { code: error.code } });
    } else {
      commit('removeReader', rootState.user);
    }
  },

  async fetchSelectedGameCharacters({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameCharacters', null);
    const { resources: characters } = await gamingApi.getCharacters(id);
    commit('updateSelectedGameCharacters', characters);
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

  async fetchSelectedGameComments({ commit }, { id, n }): Promise<void> {
    commit('updateSelectedGameComments', null);
    const data = await gamingApi.getComments(id, { number: n } as PagingQuery);
    commit('updateSelectedGameComments', data!);
  },
  async createComment({ state }, { comment, $router }): Promise<void> {
    const { id } = state.selectedGame!;
    const { error: postCommentError } = await gamingApi.createComment(id, comment);
    if (postCommentError) return Promise.reject();

    const data = await gamingApi.getComments(id, { size: 0, number: 0, skip: 0 });
    $router.push({ name: 'game-comments', params: { id, n: data.paging!.total } });
  },
  async updateComment({ commit }, { id, comment }): Promise<void> {
    const { data, error } = await gamingApi.updateComment(id, comment as Comment);
    if (!error) {
      commit('updateComment', data!.resource);
    }
  },
  async deleteComment(_, { id }) {
    await gamingApi.deleteComment(id);
  },

  async fetchSelectedGameRooms({ commit }, { id }): Promise<void> {
    commit('updateSelectedGameRooms', null);
    const { resources: rooms } = await gamingApi.getRooms(id);
    commit('updateSelectedGameRooms', rooms);
  },
};

export default actions;
