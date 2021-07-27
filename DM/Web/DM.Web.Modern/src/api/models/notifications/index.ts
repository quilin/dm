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
  AuthorLogin: string;
  GameTitle: string;
  GameId: string;
}

export interface TopicLikedData {
  AuthorLogin: string;
  TopicTitle: string;
  TopicId: string;
}
