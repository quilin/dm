import { ApiResult, Envelope, ListEnvelope, PagingQuery } from '@/api/models/common';
import { Game, AttributeSchema, Tag, Character, Room, Comment } from '@/api/models/gaming';
import { User } from '@/api/models/community';
import Api from '@/api';

export default new class {
  public async getOwnGames(): Promise<ListEnvelope<Game>> {
    const { data } = await Api.get<ListEnvelope<Game>>('games/own');
    return data!;
  }

  public async getPopularGames(): Promise<ListEnvelope<Game>> {
    const { data } = await Api.get<ListEnvelope<Game>>('games/popular');
    return data!;
  }

  public async getGame(id: string): Promise<ApiResult<Envelope<Game>>> {
    return await Api.get<Envelope<Game>>(`games/${id}/details`);
  }

  public async getCharacters(gameId: string): Promise<ListEnvelope<Character>> {
    const { data } = await Api.get<ListEnvelope<Character>>(`games/${gameId}/characters`);
    return data!;
  }

  public async getRooms(gameId: string): Promise<ListEnvelope<Room>> {
    const { data } = await Api.get<ListEnvelope<Room>>(`games/${gameId}/rooms`);
    return data!
  }

  public async getReaders(gameId: string): Promise<ListEnvelope<User>> {
    const { data } = await Api.get<ListEnvelope<User>>(`games/${gameId}/readers`);
    return data!;
  }

  public async getComments(gameId: string, query: PagingQuery): Promise<ListEnvelope<Comment>> {
    const { data } = await Api.get<ListEnvelope<Comment>>(`games/${gameId}/comments`, query);
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

  public async createCharacter(gameId: string, character: Character): Promise<ApiResult<Envelope<Character>>> {
    return await Api.post<Envelope<Character>>(`games/${gameId}/characters`, character);
  }

  public async createComment(gameId: string, comment: Comment): Promise<ApiResult<Envelope<Comment>>> {
    return await Api.post<Envelope<Comment>>(`games/${gameId}/comments`, comment);
  }

  public async subscribe(gameId: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.post<Envelope<User>>(`games/${gameId}/readers`);
  }

  public async unsubscribe(gameId: string): Promise<ApiResult<void>> {
    return await Api.delete(`games/${gameId}/readers`);
  }

  public async updateComment(commentId: string, comment: Comment): Promise<ApiResult<Envelope<Comment>>> {
    return await Api.patch<Envelope<Comment>>(`games/comments/${commentId}`, comment);
  }

  public async deleteComment(commentId: string): Promise<ApiResult<void>> {
    return await Api.delete(`games/comments/${commentId}`);
  }
}();
