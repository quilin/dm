import { defineStore } from "pinia";
import communityApi from "@/api/requests/communityApi";
import type { Review } from "@/api/models/community";
import { ref } from "vue";
import type { ListEnvelope, PagingQuery } from "@/api/models/common";

export const useReviewStore = defineStore("reviews", () => {
  const reviews = ref<ListEnvelope<Review> | null>(null);

  async function fetchReviews(number: number) {
    reviews.value = await communityApi.getReviews(
      { number } as PagingQuery,
      false
    );
  }

  async function approveReview(id: string) {
    await communityApi.updateReview(id, { approved: true } as Review);
    const review = reviews.value?.resources.find((r) => r.id === id);
    if (review) {
      review.approved = true;
    }
  }

  async function removeReview(id: string) {
    await communityApi.removeReview(id);
  }

  return {
    reviews,
    fetchReviews,
    approveReview,
    removeReview,
  };
});
