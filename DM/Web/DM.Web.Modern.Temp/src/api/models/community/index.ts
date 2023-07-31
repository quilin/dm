export interface RegisterCredentials {
  email: string;
  login: string;
  password: string;
}

export interface LoginCredentials {
  login: string;
  password: string;
  rememberMe: boolean;
}

export interface Rating {
  enabled: boolean;
  quality: number;
  quantity: number;
}

export interface User {
  login: string;
  roles: UserRole[];
  mediumPictureUrl: string;
  smallPictureUrl: string;
  rating: Rating;
  online: string;
  status: string;
  name: string;
  location: string;
  skype: string;

  originalPictureUrl: string;
  info: string;
  registration: string;
  settings?: UserSettings;
}

export enum UserRole {
  Guest = "Guest",
  Player = "Player",
  Administrator = "Administrator",
  NannyModerator = "NannyModerator",
  RegularModerator = "RegularModerator",
  SeniorModerator = "SeniorModerator",
}

export enum ColorSchema {
  Modern = "Modern",
  Pale = "Pale",
  Classic = "Classic",
  ClassicPale = "ClassicPale",
  Night = "Night",
}

export interface PagingSettings {
  postsPerPage: number;
  commentsPerPage: number;
  topicsPerPage: number;
  messagesPerPage: number;
  entitiesPerPage: number;
}

export interface UserSettings {
  nannyGreetingsMessage: string;
  colorSchema: ColorSchema;
  paging: PagingSettings;
}

export interface Poll {
  id?: string;
  ends: string;
  title: string;
  options: PollOption[];
}

export interface PollOption {
  id: string;
  text: string;
  votesCount: number;
  voted: boolean | null;
}

export interface Review {
  id: string;
  author: User;
  created: string;
  approved: boolean;
  text: string;
}
