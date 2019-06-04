import User from '@/api/models/community/User';
import Forum from '@/api/models/forum/Forum';

class LastComment {
  public created: string;
  public author: User;
}

export default class Topic {
  public id: string;
  public author: User;
  public created: string;
  public title: string;
  public description: string;
  public attached: bool;
  public closed: bool;
  public lastComment: LastComment?;
  public commentsCount: number;
  public unreadCommentsCount: number;
  public forum: Forum;
  public likes: User[];
}
