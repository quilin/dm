import { MutationTree } from 'vuex';
import GamingState from './gamingState';
import {Game, AttributeSchema, Tag, Character, Room, GameParticipation} from '@/api/models/gaming';
import {User} from '@/api/models/community';

const mutations: MutationTree<GamingState> = {
  updateOwnGames(state, payload: Game[]) {
    state.ownGames = payload;
  },
  updatePopularGames(state, payload: Game[]) {
    state.popularGames = payload;
  },
  updateSchemas(state, payload: AttributeSchema[]) {
    state.schemas = payload;
  },
  updateTags(state, payload: Tag[]) {
    state.tags = payload;
  },
  addSchema(state, payload: AttributeSchema) {
    if (state.schemas !== null) {
      state.schemas.push(payload);
    }
  },

  updateSelectedGame(state, payload: Game) {
    state.selectedGame = payload;
  },
  updateSelectedGameCharacters(state, payload: Character[]) {
    state.selectedGameCharacters = payload;
  },
  updateSelectedGameRooms(state, payload: Room[]) {
    state.selectedGameRooms = payload;
  },
  updateSelectedGameReaders(state, payload: User[]) {
    state.selectedGameReaders = payload;
  },

  addReader(state, payload: User) {
    if (state.selectedGame === null || state.selectedGameReaders === null) return;

    state.selectedGameReaders.push(payload);
    state.selectedGame.participation.push(GameParticipation.Reader);
  },
  removeReader(state, payload: User) {
    if (state.selectedGame === null || state.selectedGameReaders === null) return;

    state.selectedGameReaders = state.selectedGameReaders.filter(r => r.login !== payload.login);
    state.selectedGame.participation = state.selectedGame.participation.filter(p => p !== GameParticipation.Reader);
  },
};

export default mutations;
