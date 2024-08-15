import type { Envelope, ListEnvelope } from "@/api/models/common";
import type {
  Game,
  AttributeSchema,
  Tag,
  Character,
  Room,
} from "@/api/models/gaming";
import type { User } from "@/api/models/community";
import Api from "@/api";

export default new (class {
  public getOwnGames() {
    return Api.get<ListEnvelope<Game>>("games/own");
  }

  public async getPopularGames() {
    return Api.get<ListEnvelope<Game>>("games/popular");
  }

  public getGame(id: string) {
    return Api.get<Envelope<Game>>(`games/${id}/details`);
  }

  public getCharacters(gameId: string) {
    return Api.get<ListEnvelope<Character>>(`games/${gameId}/characters`);
  }

  public getRooms(gameId: string) {
    return Api.get<ListEnvelope<Room>>(`games/${gameId}/rooms`);
  }

  public getReaders(gameId: string) {
    return Api.get<ListEnvelope<User>>(`games/${gameId}/readers`);
  }

  public getSchemas() {
    return Api.get<ListEnvelope<AttributeSchema>>("schemata");
  }

  public getTags() {
    return Api.get<ListEnvelope<Tag>>("games/tags");
  }

  public createSchema(schema: AttributeSchema) {
    return Api.post<Envelope<AttributeSchema>>("schemata", schema);
  }

  public createGame(game: Game) {
    return Api.post<Envelope<Game>>("games", game);
  }

  public createCharacter(id: string, character: Character) {
    return Api.post<Envelope<Character>>(`games/${id}/characters`, character);
  }

  public subscribe(id: string) {
    return Api.post<Envelope<User>>(`games/${id}/readers`);
  }
  public unsubscribe(id: string) {
    return Api.delete(`games/${id}/readers`);
  }
})();
