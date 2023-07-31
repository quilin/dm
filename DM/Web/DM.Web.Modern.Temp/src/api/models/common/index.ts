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
  type: string;
  title: string;
  status: number;
  traceId: string;
}

export enum ValidationErrorCode {
  Empty = "Empty",
  Short = "Short",
  Long = "Long",
  Taken = "Taken",
  NotFound = "NotFound",
  Invalid = "Invalid",
}

export interface BadRequestError extends GeneralError {
  errors: { [field: string]: ValidationErrorCode[] };
}

export interface ApiResult<T> {
  data: T | null;
  error: GeneralError | null;
}
