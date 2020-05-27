import { ListEnvelope, ApiResult } from '@/api/models/common';
import { Game, GamesQuery } from '@/api/models/gaming/games';
import Api from '@/api';

export default new class {
  public async getOwnGames(): Promise<ApiResult<ListEnvelope<Game>>> {
    return await Api.get('games/own');
  }
  public async getGames(query: GamesQuery): Promise<ApiResult<ListEnvelope<Game>>> {
    return await Api.get('games', query);
  }
}();
