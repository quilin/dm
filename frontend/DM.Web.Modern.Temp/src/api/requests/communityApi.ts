import type { ListEnvelope, Envelope, PagingQuery } from "@/api/models/common";
import type {
  Poll,
  PollId,
  PollOptionId,
  Review,
  ReviewId,
  User,
  UserLogin,
} from "@/api/models/community";
import Api from "@/api";
import { BbRenderMode } from "../bbRenderMode";
import type { AxiosProgressEvent } from "axios";
import type { Patch, Post } from "@/api/models";

export default new (class CommunityApi {
  public getPolls(q: PagingQuery, onlyActive: boolean) {
    return Api.get<ListEnvelope<Poll>>("polls", {
      ...q,
      onlyActive,
    });
  }
  public postPollVote(pollId: PollId, optionId: PollOptionId) {
    return Api.put<Envelope<Poll>>(`polls/${pollId}?optionId=${optionId}`);
  }
  public postPoll(poll: Post<Poll>) {
    return Api.post<Envelope<Poll>>("polls", poll);
  }

  public getUsers(q: PagingQuery) {
    return Api.get<ListEnvelope<User>>("users", q);
  }

  public getUser(login: UserLogin) {
    return Api.get<Envelope<User>>(`users/${login}/details`);
  }
  public getUserForUpdate(login: UserLogin) {
    return Api.get<Envelope<User>>(
      `users/${login}/details`,
      undefined,
      BbRenderMode.Bb
    );
  }
  public updateUser(login: UserLogin, user: Patch<User>) {
    return Api.patch<Envelope<User>>(`users/${login}/details`, user);
  }
  public uploadUserPicture(
    login: UserLogin,
    files: FormData,
    progressCallback: (event: AxiosProgressEvent) => void
  ) {
    return Api.postFile<Envelope<User>>(
      `users/${login}/uploads`,
      files,
      progressCallback
    );
  }

  public getReviews(q: PagingQuery, onlyApproved: boolean) {
    return Api.get<ListEnvelope<Review>>("reviews", {
      ...q,
      onlyApproved,
    });
  }
  public updateReview(id: ReviewId, review: Patch<Review>) {
    return Api.patch<Envelope<Review>>(`reviews/${id}`, review);
  }
  public removeReview(id: ReviewId) {
    return Api.delete(`reviews/${id}`);
  }
})();
