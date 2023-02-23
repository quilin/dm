import type {
  ListEnvelope,
  Envelope,
  ApiResult,
  PagingQuery,
} from "@/api/models/common";
import type { Poll, Review, User } from "@/api/models/community";
import Api from "@/api";
import { BbRenderMode } from "../bbRenderMode";
import type { AxiosProgressEvent } from "axios";

export default new (class CommunityApi {
  public async getPolls(
    q: PagingQuery,
    onlyActive: boolean
  ): Promise<ListEnvelope<Poll>> {
    const { data } = await Api.get<ListEnvelope<Poll>>("polls", {
      ...q,
      onlyActive,
    });
    return data!;
  }
  public async postPollVote(
    pollId: string,
    optionId: string
  ): Promise<ApiResult<Envelope<Poll>>> {
    return await Api.put<Envelope<Poll>>(
      `polls/${pollId}?optionId=${optionId}`
    );
  }
  public async postPoll(poll: Poll): Promise<ApiResult<Envelope<Poll>>> {
    return await Api.post<Envelope<Poll>>("polls", poll);
  }

  public async getUsers(q: PagingQuery): Promise<ListEnvelope<User>> {
    const { data } = await Api.get<ListEnvelope<User>>("users", q);
    return data!;
  }

  public async getUser(login: string): Promise<ApiResult<Envelope<User>>> {
    return await Api.get<Envelope<User>>(`users/${login}/details`);
  }
  public async getUserForUpdate(login: string): Promise<User> {
    const { data } = await Api.get<Envelope<User>>(
      `users/${login}/details`,
      undefined,
      BbRenderMode.Bb
    );
    return data!.resource;
  }
  public async updateUser(
    login: string,
    user: User
  ): Promise<ApiResult<Envelope<User>>> {
    return await Api.patch<Envelope<User>>(`users/${login}/details`, user);
  }
  public async uploadUserPicture(
    login: string,
    files: FormData,
    progressCallback: (event: AxiosProgressEvent) => void
  ): Promise<Envelope<User>> {
    const { data } = await Api.postFile<Envelope<User>>(
      `users/${login}/uploads`,
      files,
      progressCallback
    );
    return data!;
  }

  public async getReviews(
    q: PagingQuery,
    onlyApproved: boolean
  ): Promise<ListEnvelope<Review>> {
    const { data } = await Api.get<ListEnvelope<Review>>("reviews", {
      ...q,
      onlyApproved,
    });
    return data!;
  }
  public async updateReview(
    id: string,
    review: Review
  ): Promise<ApiResult<Envelope<Review>>> {
    return await Api.patch<Envelope<Review>>(`reviews/${id}`, review);
  }
  public async removeReview(id: string): Promise<ApiResult<Envelope<Review>>> {
    return await Api.delete<Envelope<Review>>(`reviews/${id}`);
  }
})();
