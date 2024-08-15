import type {
  ApiResult,
  Envelope,
  ListEnvelope,
  PagingQuery,
} from "@/api/models/common";
import type { User } from "@/api/models/community";
import type { Comment, Forum, Topic } from "@/api/models/forum";
import Api from "@/api";
import { BbRenderMode } from "@/api/bbRenderMode";

export default new (class ForumApi {
  public async getFora(): Promise<ApiResult<ListEnvelope<Forum>>> {
    return await Api.get("fora");
  }

  public async getForum(id: string): Promise<ApiResult<Envelope<Forum>>> {
    return await Api.get(`fora/${id}`);
  }

  public async getNews(): Promise<ApiResult<ListEnvelope<Topic>>> {
    return await Api.get("fora/Новости проекта/topics", { size: 3 });
  }

  public async getModerators(
    id: string
  ): Promise<ApiResult<ListEnvelope<User>>> {
    return await Api.get(`fora/${id}/moderators`);
  }

  public async getTopics(
    id: string,
    q: PagingQuery,
    attached: boolean
  ): Promise<ApiResult<ListEnvelope<Topic>>> {
    return await Api.get(`fora/${id}/topics`, { ...q, attached });
  }

  public async postTopic(topic: Topic): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.post(`fora/${topic.forum.id}/topics`, topic);
  }

  public async getTopic(id: string): Promise<ApiResult<Envelope<Topic>>> {
    return await Api.get(`topics/${id}`);
  }

  public async markAllTopicsAsRead(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`fora/${id}/comments/unread`);
  }

  public async markTopicAsRead(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`topics/${id}/comments/unread`);
  }

  public async getComments(
    id: string,
    q: PagingQuery
  ): Promise<ApiResult<ListEnvelope<Comment>>> {
    return await Api.get(`topics/${id}/comments`, q);
  }

  public async postComment(
    id: string,
    comment: Comment
  ): Promise<ApiResult<Envelope<Comment>>> {
    return await Api.post(`topics/${id}/comments`, comment);
  }

  public async updateComment(
    id: string,
    comment: Comment
  ): Promise<ApiResult<Envelope<Comment>>> {
    return await Api.patch(`forum/comments/${id}`, comment);
  }

  public async deleteComment(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`forum/comments/${id}`);
  }

  public async getCommentForUpdate(id: string): Promise<Comment> {
    const { data } = await Api.get<Envelope<Comment>>(
      `forum/comments/${id}`,
      undefined,
      BbRenderMode.Bb
    );

    return data!.resource;
  }

  public async postCommentLike(id: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.post(`forum/comments/${id}/likes`);
  }
  public async deleteCommentLike(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`forum/comments/${id}/likes`);
  }

  public async postTopicLike(id: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.post(`topics/${id}/likes`);
  }
  public async deleteTopicLike(id: string): Promise<ApiResult<void>> {
    return await Api.delete(`topics/${id}/likes`);
  }
})();
