import type { Id, Served } from "@/api/models";

export type PollId = Id<string>;
export type Poll = {
  id: Served<PollId>;
  ends: string;
  title: string;
  options: PollOption[];
};

export type PollOptionId = Id<string>;
export type PollOption = {
  id: Served<PollOptionId>;
  text: string;
  votesCount: Served<number>;
  voted: Served<boolean | null>;
};
