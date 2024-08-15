import type { User } from "@/api/models/community";
import type { Id, Served } from "@/api/models";

export type ForumId = Id<string>;
export type Forum = {
  id: Served<ForumId>;
  unreadTopicsCount: Served<number>;
};

export type LastComment = {
  created: string;
  author: User;
};

export type TopicId = Id<string>;
export type Topic = {
  id: Served<TopicId>;
  author: Served<User>;
  created: Served<string>;
  title: string;
  description: string;
  attached: boolean;
  closed: boolean;
  lastComment: Served<LastComment | null>;
  commentsCount: Served<number>;
  unreadCommentsCount: Served<number>;
  forum: Forum;
  likes: Served<User[]>;
};

export type CommentId = Id<string>;
export type Comment = {
  id: Served<CommentId>;
  author: Served<User>;
  created: Served<string>;
  updated: Served<string | null>;
  text: string;
  likes: Served<User[]>;
};
