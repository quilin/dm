import type { Id, Served } from "@/api/models";
import type { User } from "@/api/models/community/index";

export type ReviewId = Id<string>;
export type Review = {
  id: Served<ReviewId>;
  author: Served<User>;
  created: Served<string>;
  approved: boolean;
  text: string;
};
