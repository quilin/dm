import type { PagingQuery } from "@/api/models/common";
import type { User } from "@/api/models/community";
import type { AttributeSchema } from "@/api/models/gaming/attributes";

export enum GameStatus {
  Closed = "Closed",
  Finished = "Finished",
  Frozen = "Frozen",
  Requirement = "Requirement",
  Draft = "Draft",
  Active = "Active",
  RequiresModeration = "RequiresModeration",
  Moderation = "Moderation",
}

export enum GameParticipation {
  None = "None",
  Reader = "Reader",
  Player = "Player",
  Moderator = "Moderator",
  PendingAssistant = "PendingAssistant",
  Authority = "Authority",
  Owner = "Owner",
}

export interface Tag {
  id: string;
  title: string;
  category: string;
}

export enum CommentariesAccessMode {
  Public = "Public",
  Readonly = "Readonly",
  Private = "Private",
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
  participation: GameParticipation[];
  released: string;

  master: User;
  assistant: User | null;
  pendingAssistant: User | null;
  nanny: User | null;
  notes: string;
  info: string;

  tags: Tag[];
  privacySettings: GamePrivacySettings;
  schema: AttributeSchema | null;

  unreadPostsCount: number;
  unreadCommentsCount: number;
  unreadCharactersCount: number;
}

export interface GamesQuery extends PagingQuery {
  statuses: GameStatus[];
}
