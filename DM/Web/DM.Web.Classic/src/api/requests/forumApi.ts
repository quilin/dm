import ListEnvelope from '@/api/models/common/listEnvelope';
import Forum from '@/api/models/forum/forum';
import Topic from '@/api/models/forum/topic';
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
}();
