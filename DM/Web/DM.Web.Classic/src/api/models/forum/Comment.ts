import User from '@/api/models/community/User';

export default class Comment {
  public id: string;
  public author: User;
  public created: string;
  public updated: string?;
  public text: string;
  public likes: User[];
}
