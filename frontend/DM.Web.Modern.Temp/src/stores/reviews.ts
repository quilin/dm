import { defineStore } from "pinia";
import { ref } from "vue";
import type { ListEnvelope } from "@/api/models/common";
import type { Review, ReviewId } from "@/api/models/community";
import communityApi from "@/api/requests/communityApi";

export const useReviewStore = defineStore("reviews", () => {
  const reviews = ref<ListEnvelope<Review> | null>(null);

  async function fetchReviews(number: number) {
    const { data } = await communityApi.getReviews({ number }, false);
    reviews.value = data;
  }

  async function approveReview(id: ReviewId) {
    await communityApi.updateReview(id, { approved: true });
    const review = reviews.value?.resources.find((r) => r.id === id);
    if (review) {
      review.approved = true;
    }
  }

  async function removeReview(id: ReviewId) {
    await communityApi.removeReview(id);
  }

  async function getRandomReview() {
    const { data } = await communityApi.getReviews({ size: 0 }, true);
    const { paging } = data!;

    const randomNumber = Math.floor(Math.random() * paging!.total);
    const { data: reviews } = await communityApi.getReviews(
      { size: 1, skip: randomNumber },
      true,
    );
    const { resources } = reviews!;
    return resources[0];
  }

  return {
    reviews,
    fetchReviews,
    approveReview,
    removeReview,
    getRandomReview,
  };
});
