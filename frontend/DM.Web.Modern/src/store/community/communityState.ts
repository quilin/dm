import { Poll, Review, User } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

export default interface CommunityState {
  activePolls: Poll[] | null;
  polls: ListEnvelope<Poll> | null;

  users: ListEnvelope<User> | null;
  selectedUser: { view: User; edit: User } | null;

  reviews: ListEnvelope<Review> | null;
}
