import { ApiResult, Envelope, ListEnvelope } from '@/api/models/common';
import { Game, AttributeSchema, Tag } from '@/api/models/gaming';
import Api from '@/api';

export default new class {
  public async getOwnGames(): Promise<ListEnvelope<Game>> {
    const { data } = await Api.get<ListEnvelope<Game>>('games/own');
    return data!;
  }

  public async getSchemas(): Promise<ListEnvelope<AttributeSchema>> {
    const { data } = await Api.get<ListEnvelope<AttributeSchema>>('schemata');
    return data!;
  }
  public async getTags(): Promise<ListEnvelope<Tag>> {
    const { data } = await Api.get<ListEnvelope<Tag>>('games/tags');
    return data!;
  }

  public async createSchema(schema: AttributeSchema): Promise<ApiResult<Envelope<AttributeSchema>>> {
    return await Api.post<Envelope<AttributeSchema>>('schemata', schema);
  }
  public async createGame(game: Game): Promise<ApiResult<Envelope<Game>>> {
    return await Api.post<Envelope<Game>>('games', game);
  }
}();
