export enum NotificationType {
  NewCharacter = 361
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