import ListEnvelope from '@/api/models/common/listEnvelope';
import User from '@/api/models/community/user';
import { Forum, Topic } from '@/api/models/forum';
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
}();
