export interface Envelope<T> {
  resource: T;
}

export interface Paging {
  pages: number;
  current: number;
  size: number;
  number: number;
}

export interface ListEnvelope<T> {
  resources: T[];
  paging: Paging | null;
}

export interface GeneralError {
  message: string;
}

export interface BadRequestError extends GeneralError {
  invalidProperties: { [field: string]: string };
}
