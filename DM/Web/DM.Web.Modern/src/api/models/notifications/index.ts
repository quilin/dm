export enum NotificationType {
  NewCharacter = 361,
  LikedTopic = 104,
}

export interface UserNotification {
  id: string;
  eventType: NotificationType;
  payload: any;
}

export interface NewCharacterData {
  authorLogin: string;
  gameTitle: string;
  gameId: string;
}

export interface TopicLikedData {
  authorLogin: string;
  topicTitle: string;
  topicId: string;
}
