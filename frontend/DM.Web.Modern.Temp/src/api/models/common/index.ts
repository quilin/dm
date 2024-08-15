export type Envelope<T> = {
  resource: T;
};

export type Paging = {
  pages: number;
  current: number;
  size: number;
  number: number;
  total: number;
};

export type PagingQuery = {
  skip?: number;
  size?: number;
  number?: number;
};

export type ListEnvelope<T> = {
  resources: T[];
  paging: Paging | null;
};

export type GeneralError = {
  type: string;
  title: string;
  status: number;
  traceId: string;
};

export enum ValidationErrorCode {
  Empty = "Empty",
  Short = "Short",
  Long = "Long",
  Taken = "Taken",
  NotFound = "NotFound",
  Invalid = "Invalid",
}

export type BadRequestError = GeneralError & {
  errors: { [field: string]: ValidationErrorCode[] };
};

export type ApiResult<T> = {
  data: T | null;
  error: GeneralError | null;
};
