import { GetterTree } from 'vuex';
import GamingState from './gamingState';
import RootState from './../rootState';

const getters: GetterTree<GamingState, RootState> = {
  ownGames: (state) => state.ownGames,
  popularGames: (state) => state.popularGames,

  schemas: (state) => state.schemas,
  tags: (state) => state.tags,

  selectedGame: (state) => state.selectedGame,
  selectedGameCharacters: (state) => state.selectedGameCharacters,
  selectedGameRooms: (state) => state.selectedGameRooms,
  selectedGameReaders: (state) => state.selectedGameReaders,
};

export default getters;
