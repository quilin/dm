import type { Id, Served } from "@/api/models";
import type { UserLogin } from "@/api/models/community";
import type { TopicId } from "@/api/models/forum";
import type { GameId } from "@/api/models/gaming";

export enum NotificationType {
  NewCharacter = 361,
  LikedTopic = 104,
}

export type NotificationId = Id<string>;
export type UserNotification = {
  id: Served<NotificationId>;
  eventType: Served<NotificationType>;
  payload: Served<any>;
};

export type NewCharacterData = {
  authorLogin: Served<UserLogin>;
  gameTitle: Served<string>;
  gameId: Served<GameId>;
};

export type TopicLikedData = {
  authorLogin: Served<UserLogin>;
  topicTitle: Served<string>;
  topicId: Served<TopicId>;
};
