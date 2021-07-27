import {Game, AttributeSchema, Tag, Character, Room} from '@/api/models/gaming';
import {User} from '@/api/models/community';

export default interface GamingState {
  ownGames: Game[] | null;
  popularGames: Game[] | null;

  schemas: AttributeSchema[] | null;
  tags: Tag[] | null;

  selectedGame: Game | null;
  selectedGameCharacters: Character[] | null;
  selectedGameRooms: Room[] | null;
  selectedGameReaders: User[] | null;
}
