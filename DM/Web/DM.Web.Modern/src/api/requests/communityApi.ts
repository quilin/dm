import { ListEnvelope, Envelope, ApiResult } from '@/api/models/common';
import { Poll, User } from '@/api/models/community';
import Api from '@/api';

export default new class CommunityApi {
  public async getPolls(onlyActive: boolean): Promise<ListEnvelope<Poll>> {
    const { data } = await Api.get<ListEnvelope<Poll>>('polls', { onlyActive });
    return  data!;
  }

  public async postPollVote(pollId: string, optionId: string): Promise<ApiResult<Envelope<Poll>>> {
    return await Api.put<Envelope<Poll>>(`polls/${pollId}?optionId=${optionId}`);
  }

  public async getUsers(n: number): Promise<ListEnvelope<User>> {
    const { data } = await Api.get<ListEnvelope<User>>('users', { number: n });
    return data!;
  }
}();
