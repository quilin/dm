import { ListEnvelope, Envelope, ApiResult } from '@/api/models/common';
import { Poll, User } from '@/api/models/community';
import Api from '@/api';
import { BbRenderMode } from '../bbRenderMode';

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

  public async getUser(login: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.get<Envelope<User>>(`users/${login}`);
  }
  public async getUserForUpdate(login: string): Promise<User> {
    const { data } = await Api.get<Envelope<User>>(`users/${login}`, undefined, BbRenderMode.Bb);
    return data!.resource;
  }
  public async updateUser(login: string, user: User): Promise<ApiResult<Envelope<User>>> {
    return await Api.patch<Envelope<User>>(`users/${login}`, user);
  }
}();
