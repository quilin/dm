export interface Envelope<T> {
  resource: T;
}

export interface Paging {
  pages: number;
  current: number;
  size: number;
  number: number;
  total: number;
}

export interface PagingQuery {
  skip: number | null;
  size: number | null;
  number: number | null;
}

export interface ListEnvelope<T> {
  resources: T[];
  paging: Paging | null;
}

export interface GeneralError {
  message: string;
  code: number;
}

export interface BadRequestError extends GeneralError {
  invalidProperties: { [field: string]: string };
}

export interface ApiResult<T> {
  data: T | null;
  error: GeneralError | null;
}
