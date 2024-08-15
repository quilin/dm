import type { User } from "@/api/models/community";
import type { Id, Served } from "@/api/models";

export enum CharacterStatus {
  Registration = "Registration",
  Declined = "Declined",
  Active = "Active",
  Dead = "Dead",
  Left = "Left",
}

export enum Alignment {
  LawfulGood = "LawfulGood",
  NeutralGood = "NeutralGood",
  ChaoticGood = "ChaoticGood",
  LawfulNeutral = "LawfulNeutral",
  TrueNeutral = "TrueNeutral",
  ChaoticNeutral = "ChaoticNeutral",
  LawfulEvil = "LawfulEvil",
  NeutralEvil = "NeutralEvil",
  ChaoticEvil = "ChaoticEvil",
}

export type CharacterPrivacySettings = {
  isNpc: boolean;
  editByMaster: boolean;
  editPostByMaster: boolean;
};

export type CharacterAttributeId = Id<string>;
export type CharacterAttribute = {
  id: Served<CharacterAttributeId>;
  title: Served<string>;
  value: string;
  modifier: Served<string>;
  inconsistent: Served<string>;
};

export type CharacterId = Id<string>;
export type Character = {
  id: Served<CharacterId>;
  author: Served<User>;
  status: CharacterStatus;
  name: string;
  race: string;
  class: string;
  alignment: Alignment;
  pictureUrl: Served<string>;
  appearance: string;
  temper: string;
  story: string;
  skills: string;
  inventory: string;
  privacy: CharacterPrivacySettings;
  attributes: Served<CharacterAttribute[]>;
  totalPostsCount: Served<number>;
};
