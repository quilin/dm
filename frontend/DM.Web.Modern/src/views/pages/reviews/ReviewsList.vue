<template>
  <div>

    <paging v-if="reviews"
      :paging="reviews.paging"
      :to="{ name: 'about', params: $route.params }" />

    <loader v-if="reviews === null" :big="true" />
    <template v-else-if="reviews.resources.length">
      <review v-for="review in reviews.resources" :key="review.id"
        :review="review" :controls="true" />
    </template>
    <div v-else>...ничего пока не написали</div>

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';
import { ListEnvelope } from '@/api/models/common';
import { Review } from '@/api/models/community';
import ReviewComponent from './Review.vue';

@Component({
  components: {
    Review: ReviewComponent,
  },
})
export default class ReviewsList extends Vue {
  @Getter('community/reviews')
  private reviews!: ListEnvelope<Review> | null;
}
</script>

<style lang="stylus" scoped>
</style>