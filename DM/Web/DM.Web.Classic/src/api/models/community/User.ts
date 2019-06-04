import Rating from '@/api/models/community/Rating';

export default interface User {
  login: string;
  roles: string[];
  profilePictureUrl: string;
  rating: Rating;
  online: string;
}
