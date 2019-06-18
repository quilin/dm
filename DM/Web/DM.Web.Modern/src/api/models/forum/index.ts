import { User } from '@/api/models/community';

export interface Forum {
  id: string;
  unreadTopicsCount: number;
}

export interface LastComment {
  created: string;
  author: User;
}

export interface Topic {
  id?: string;
  author?: User;
  created?: string;
  title: string;
  description?: string;
  attached?: boolean;
  closed?: boolean;
  lastComment?: LastComment | null;
  commentsCount?: number;
  unreadCommentsCount?: number;
  forum: Forum;
  likes?: User[];
}

export interface Comment {
  id: string;
  author: User;
  created: string;
  updated: string | null;
  text: string;
  likes: User[];
}
