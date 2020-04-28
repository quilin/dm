import { User } from '@/api/models/community';

export enum AttributeSchemaType {
  Public = 0,
  Private = 1,
}

export enum AttributeSpecificationType {
  Number = 0,
  String = 1,
  List = 2,
}

export interface AttributeValueSpecification {
  value: string;
  modifier: number | null;
}

export interface AttributeSpecification {
  id: string;
  title: string;
  required: boolean;
  type: AttributeSpecificationType;
  minValue: number | null;
  maxValue: number | null;
  maxLength: number | null;
  values: AttributeValueSpecification[] | null;
}

export interface AttributeSchema {
  id: string;
  title: string;
  author: User | null;
  type: AttributeSchemaType;
  specification: AttributeSpecification[];
}
