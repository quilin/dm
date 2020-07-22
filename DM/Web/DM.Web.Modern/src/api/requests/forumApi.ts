import {ListEnvelope, Envelope, ApiResult, PagingQuery} from '@/api/models/common';
import { User } from '@/api/models/community';
import { Forum, Topic, Comment } from '@/api/models/forum';
import Api from '@/api';

export default new class ForumApi {
  public async getFora(): Promise<ApiResult<ListEnvelope<Forum>>> {
    return await Api.get('fora');
  }
  public async getForum(id: string): Promise<ApiResult<Envelope<Forum>>> {
    return await Api.get(`fora/${id}`);
  }
  public async getNews(): Promise<ApiResult<ListEnvelope<Topic>>> {
    return await Api.get('fora/Новости проекта/topics', {size: 3});
  }
  public async getModerators(id: string): Promise<ApiResult<ListEnvelope<User>>> {
    return await Api.get(`fora/${id}/moderators`);
  }
  public async getTopics(id: string, q: PagingQuery, attached: boolean): Promise<ApiResult<ListEnvelope<Topic>>> {
    return await Api.get(`fora/${id}/topics`, { ...q, attached });
  }
  public async postTopic(topic: Topic): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.post(`fora/${topic.forum.id}/topics`, topic);
  }

  public async getTopic(id: string): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.get(`topics/${id}`);
  }
  public async getComments(id: string, q: PagingQuery): Promise<ApiResult<ListEnvelope<Comment>>> {
    return await Api.get(`topics/${id}/comments`, q);
  }
  public async markAllTopicsAsRead(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`fora/${id}/comments/unread`);
  }
}();
