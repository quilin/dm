import Rating from '@/api/models/community/Rating';

export default class User {
  public login: string;
  public roles: string[];
  public profilePictureUrl: string;
  public rating: Rating;
  public online: string;
}
