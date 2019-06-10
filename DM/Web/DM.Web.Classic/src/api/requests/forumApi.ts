import { ListEnvelope, Envelope } from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';
import Api from '@/api';

export default new class ForumApi {
  public async get(): Promise<ListEnvelope<Forum>> {
    return await Api.get('fora');
  }
  public async getNews(): Promise<ListEnvelope<Topic>> {
    return await Api.get('fora/Новости проекта/topics', {
      size: 3,
    });
  }

  public async getModerators(id: string): Promise<ListEnvelope<User>> {
    return await Api.get(`fora/${id}/moderators`);
  }
  public async getTopics(id: string, attached: boolean, n: number): Promise<ListEnvelope<Topic>> {
    return await Api.get(`fora/${id}/topics`, { number: n, attached });
  }
  public async postTopic(topic: Topic): Promise<Envelope<Topic>> | GeneralError {
    return await Api.post(`fora/${topic.forum.id}/topics`, topic);
  }

  public async getTopic(id: string): Promise<Envelope<Topic>> {
    return await Api.get(`topics/${id}`);
  }
  public async getComments(id: string, n: number): Promise<ListEnvelope<Comment>> {
    return await Api.get(`topics/${id}/comments`, { number: n });
  }
}();
