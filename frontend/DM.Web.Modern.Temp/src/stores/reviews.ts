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

  return {
    reviews,
    fetchReviews,
    approveReview,
    removeReview,
  };
});
