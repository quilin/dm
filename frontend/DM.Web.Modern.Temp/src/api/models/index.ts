// Branded type
type InternalServed<T> = T & { __type: T };

// Idempotent generics (Served<number> = Served<Served<number>>)
export type Served<T> = T extends InternalServed<infer U>
  ? InternalServed<U>
  : InternalServed<T>;

type IfEquals<X, Y, A, B> = (<T>() => T extends X ? 1 : 2) extends <
  T
>() => T extends Y ? 1 : 2
  ? A
  : B;

export type Id<T> = Served<T>;

export type Post<T> = {
  [P in keyof T as IfEquals<T[P], Served<T[P]>, never, P>]: T[P];
};

export type Patch<T> = {
  [P in keyof T as IfEquals<T[P], Served<T[P]>, never, P>]?: T[P];
};
