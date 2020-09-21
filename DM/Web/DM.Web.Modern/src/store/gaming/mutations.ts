import { MutationTree } from 'vuex';
import GamingState from './gamingState';
import { Game, AttributeSchema, Tag } from '@/api/models/gaming';

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
};

export default mutations;
