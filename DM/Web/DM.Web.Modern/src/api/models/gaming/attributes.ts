import { User } from '@/api/models/community';

export enum AttributeSchemaType {
  Public = 'Public',
  Private = 'Private',
}

export enum AttributeSpecificationType {
  Number = 'Number',
  String = 'String',
  List = 'List',
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
  minValue: number | null | undefined;
  maxValue: number | null | undefined;
  maxLength: number | null | undefined;
  values: AttributeValueSpecification[] | null | undefined;
}

export interface AttributeSchema {
  id: string | null;
  title: string;
  author: User | null;
  type: AttributeSchemaType;
  specifications: AttributeSpecification[];
}
