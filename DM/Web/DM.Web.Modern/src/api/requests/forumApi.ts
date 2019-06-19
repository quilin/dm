import { ListEnvelope, Envelope, ApiResult } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';
import Api from '@/api';

export default new class ForumApi {
  public async getFora(): Promise<ListEnvelope<Forum>> {
    const { data } = await Api.get('fora');
    return data;
  }
  public async getForum(id: string): Promise<ApiResult<Envelope<Forum>>> {
    return await Api.get(`fora/${id}`);
  }
  public async getNews(): Promise<ListEnvelope<Topic>> {
    const { data } = await Api.get('fora/Новости проекта/topics', {
      size: 3,
    });
    return data;
  }

  public async getModerators(id: string): Promise<ApiResult<ListEnvelope<User>>> {
    return await Api.get(`fora/${id}/moderators`);
  }
  public async getTopics(id: string, attached: boolean, n: number): Promise<ApiResult<ListEnvelope<Topic>>> {
    return await Api.get(`fora/${id}/topics`, { number: n, attached });
  }
  public async postTopic(topic: Topic): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.post(`fora/${topic.forum.id}/topics`, topic);
  }

  public async getTopic(id: string): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.get(`topics/${id}`);
  }
  public async getComments(id: string, n: number): Promise<ApiResult<ListEnvelope<Comment>>> {
    return await Api.get(`topics/${id}/comments`, { number: n });
  }
}();
