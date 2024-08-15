import type { PagingQuery } from "@/api/models/common";
import type { User } from "@/api/models/community";
import type { AttributeSchema } from "@/api/models/gaming/attributes";
import type { Id, Served } from "@/api/models";

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

export type TagId = Id<string>;
export type Tag = {
  id: Served<TagId>;
  title: Served<string>;
  category: Served<string>;
};

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

export type GameId = Id<string>;
export type Game = {
  id: Served<GameId>;
  title: string;
  system: string;
  setting: string;
  status: GameStatus;
  participation: Served<GameParticipation[]>;
  released: Served<string>;

  master: Served<User>;
  assistant: User | null;
  pendingAssistant: Served<User | null>;
  nanny: Served<User | null>;
  notes: string;
  info: string;

  tags: Tag[];
  privacySettings: GamePrivacySettings;
  schema: AttributeSchema | null;

  unreadPostsCount: Served<number>;
  unreadCommentsCount: Served<number>;
  unreadCharactersCount: Served<number>;
};

export interface GamesQuery extends PagingQuery {
  statuses: GameStatus[];
}
