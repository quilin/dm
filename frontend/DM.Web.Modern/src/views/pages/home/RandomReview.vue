<template>
  <div>
    <div class="content-title">Случайный отзыв</div>
    <review v-if="review" :review="review" :controls="false" />
    <loader v-else />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Review } from '@/api/models/community';
import communityApi from '@/api/requests/communityApi';
import { PagingQuery } from '@/api/models/common';
import ReviewComponent from '@/views/pages/reviews/Review.vue';

@Component({
  components: {
    Review: ReviewComponent,
  },
})
export default class RandomReview extends Vue {
  private review: Review | null = null;

  private async mounted(): Promise<void> {
    const { paging } = await communityApi.getReviews({ size: 0 } as PagingQuery, true);
    const randomIndex = Math.max(0, Math.round(Math.random() * (paging!.total - 1)));
    const { resources } = await communityApi.getReviews({ size: 1, skip: randomIndex } as PagingQuery, true);
    this.review = resources![0];
  }
}
</script>

<style lang="stylus" scoped>
.container
  margin $medium 0
</style>