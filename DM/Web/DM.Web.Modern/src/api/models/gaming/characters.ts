import { User } from '@/api/models/community';

export enum CharacterStatus
{
  Registration = 'Registration',
  Declined = 'Declined',
  Active = 'Active',
  Dead = 'Dead',
  Left = 'Left',
}

export enum Alignment
{
  LawfulGood = 'LawfulGood',
  NeutralGood = 'NeutralGood',
  ChaoticGood = 'ChaoticGood',
  LawfulNeutral = 'LawfulNeutral',
  TrueNeutral = 'TrueNeutral',
  ChaoticNeutral = 'ChaoticNeutral',
  LawfulEvil = 'LawfulEvil',
  NeutralEvil = 'NeutralEvil',
  ChaoticEvil = 'ChaoticEvil',
}

export interface CharacterPrivacySettings
{
  isNpc: boolean;
  editByMaster: boolean;
  editPostByMaster: boolean;
}

export interface CharacterAttribute
{
  id: string;
  title: string;
  value: string;
  modifier: string;
  inconsistent: string;
}

export interface Character {
  id: string;
  author: User;
  status: CharacterStatus;
  name: string;
  race: string;
  class: string;
  alignment: Alignment;
  pictureUrl: string;
  appearance: string;
  temper: string;
  story: string;
  skills: string;
  inventory: string;
  privacy: CharacterPrivacySettings;
  attributes: CharacterAttribute[];
  totalPostsCount: number;
}