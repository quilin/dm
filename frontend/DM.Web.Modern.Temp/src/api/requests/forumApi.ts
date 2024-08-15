import type { Envelope, ListEnvelope, PagingQuery } from "@/api/models/common";
import type { User } from "@/api/models/community";
import type {
  Comment,
  CommentId,
  Forum,
  ForumId,
  Topic,
  TopicId,
} from "@/api/models/forum";
import Api from "@/api";
import { BbRenderMode } from "@/api/bbRenderMode";
import type { Patch, Post } from "@/api/models";

export default new (class ForumApi {
  public getFora() {
    return Api.get<ListEnvelope<Forum>>("fora");
  }

  public getForum(id: ForumId) {
    return Api.get<Envelope<Forum>>(`fora/${id}`);
  }

  public getNews() {
    return Api.get<ListEnvelope<Topic>>("fora/Новости проекта/topics", {
      size: 3,
    });
  }

  public getModerators(id: ForumId) {
    return Api.get<ListEnvelope<User>>(`fora/${id}/moderators`);
  }

  public getTopics(id: ForumId, q: PagingQuery, attached: boolean) {
    return Api.get<ListEnvelope<Topic>>(`fora/${id}/topics`, {
      ...q,
      attached,
    });
  }

  public createTopic(id: ForumId, topic: Post<Topic>) {
    return Api.post<Envelope<Topic>>(`fora/${id}/topics`, topic);
  }

  public getTopic(id: TopicId) {
    return Api.get<Envelope<Topic>>(`topics/${id}`);
  }

  public markAllTopicsAsRead(id: ForumId) {
    return Api.delete(`fora/${id}/comments/unread`);
  }

  public markTopicAsRead(id: TopicId) {
    return Api.delete(`topics/${id}/comments/unread`);
  }

  public getComments(id: TopicId, q: PagingQuery) {
    return Api.get<ListEnvelope<Comment>>(`topics/${id}/comments`, q);
  }

  public createComment(id: TopicId, comment: Post<Comment>) {
    return Api.post<Envelope<Comment>>(`topics/${id}/comments`, comment);
  }

  public updateComment(id: CommentId, comment: Patch<Comment>) {
    return Api.patch<Envelope<Comment>>(`forum/comments/${id}`, comment);
  }

  public deleteComment(id: CommentId) {
    return Api.delete(`forum/comments/${id}`);
  }

  public getCommentForUpdate(id: CommentId) {
    return Api.get<Envelope<Comment>>(
      `forum/comments/${id}`,
      undefined,
      BbRenderMode.Bb
    );
  }

  public postCommentLike(id: CommentId) {
    return Api.post<Envelope<User>>(`forum/comments/${id}/likes`);
  }
  public deleteCommentLike(id: CommentId) {
    return Api.delete(`forum/comments/${id}/likes`);
  }

  public postTopicLike(id: TopicId) {
    return Api.post<Envelope<User>>(`topics/${id}/likes`);
  }
  public deleteTopicLike(id: TopicId) {
    return Api.delete(`topics/${id}/likes`);
  }
})();
