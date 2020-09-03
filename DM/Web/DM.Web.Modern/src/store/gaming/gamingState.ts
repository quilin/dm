import { Game, AttributeSchema, Tag } from '@/api/models/gaming';

export default interface GamingState {
  ownGames: Game[] | null;
  schemas: AttributeSchema[] | null;
  tags: Tag[] | null;
}
