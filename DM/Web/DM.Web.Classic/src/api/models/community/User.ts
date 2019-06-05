import Rating from '@/api/models/community/rating';

export default interface User {
  login: string;
  roles: string[];
  profilePictureUrl: string;
  rating: Rating;
  online: string;
}
