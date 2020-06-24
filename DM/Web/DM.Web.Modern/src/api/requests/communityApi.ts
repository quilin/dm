import { ListEnvelope } from '@/api/models/common';
import { Poll, User } from '@/api/models/community';
import Api from '@/api';

export default new class CommunityApi {
  public async getPolls(onlyActive: boolean): Promise<ListEnvelope<Poll>> {
    const { data } = await Api.get<ListEnvelope<Poll>>('polls', { onlyActive });
    return  data!;
  }

  public async getUsers(n: number): Promise<ListEnvelope<User>> {
    const { data } = await Api.get<ListEnvelope<User>>('users', { number: n });
    return data!;
  }
}();
