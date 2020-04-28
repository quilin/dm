import { Game } from '@/api/models/gaming/games';

export default interface GamingState {
  ownGames: Game[] | null;
}
