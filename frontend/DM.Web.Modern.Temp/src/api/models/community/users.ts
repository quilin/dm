import type { Id, Served } from "@/api/models";
import type { UserSettings } from "@/api/models/community/user-settings";

export type Rating = {
  enabled: boolean;
  quality: number;
  quantity: number;
};

export type UserLogin = Id<string>;
export type User = {
  login: Served<UserLogin>;
  roles: Served<UserRole[]>;
  mediumPictureUrl: Served<string>;
  smallPictureUrl: Served<string>;
  rating: Served<Rating>;
  online: Served<string>;
  status: string;
  name: string;
  location: string;
  skype: string;

  originalPictureUrl: Served<string>;
  info: string;
  registration: Served<string>;
  settings: Served<UserSettings>;
};

export enum UserRole {
  Guest = "Guest",
  Player = "Player",
  Administrator = "Administrator",
  NannyModerator = "NannyModerator",
  RegularModerator = "RegularModerator",
  SeniorModerator = "SeniorModerator",
}
