import { PagingQuery } from '@/api/models/common';
import { User } from '@/api/models/community';
import { AttributeSchema } from '@/api/models/gaming/attributes';

export enum GameStatus {
  Closed = 0,
  Finished = 1,
  Frozen = 2,
  Requirement = 3,
  Draft = 4,
  Active = 5,
  RequiresModeration = 6,
  Moderation = 7,
}

export enum GameParticipation {
  None = 0,
  Reader = 1 << 0,
  Player = 1 << 1,
  Moderator = 1 << 2,
  PendingAssistant = 1 << 3,
  Authority = 1 << 4,
  Owner = 1 << 5,
}

export interface Tag {
  id: string;
  title: string;
  category: string;
}

export enum CommentariesAccessMode {
  Public = 0,
  Readonly = 1,
  Private = 2,
}

export interface GamePrivacySettings {
  viewTemper: boolean;
  viewStory: boolean;
  viewSkills: boolean;
  viewInventory: boolean;
  viewPrivates: boolean;
  viewDice: boolean;
  commentariesAccess: CommentariesAccessMode;
}

export interface Game {
  id: string;
  title: string;
  system: string;
  setting: string;
  status: GameStatus;
  participation: GameParticipation;
  released: string;

  master: User;
  assistant: User | null;
  pendingAssistant: User | null;
  nanny: User | null;
  notes: string;
  info: string;

  tags: Tag[];
  privacySettings: GamePrivacySettings;
  schema: AttributeSchema;

  unreadPostsCount: number;
  unreadCommentsCount: number;
  unreadCharactersCount: number;
}

export interface GamesQuery extends PagingQuery {
  statuses: GameStatus[];
}
