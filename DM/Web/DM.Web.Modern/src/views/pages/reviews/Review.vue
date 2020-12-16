<template>
  <div class="review">
    <div class="review-text" v-html="review.text" />
    <div class="review-info">
      <user-link :user="review.author" />
      <div v-if="canAdministrate" class="review-controls">
        <span v-if="!review.approved">Ожидает проверки</span>
        <template v-if="!loading">
          <a v-if="!review.approved" @click="approve"><icon :font="IconType.Tick" /> Принять</a>
          <a @click="remove"><icon :font="IconType.Close" /> Отклонить</a>
        </template>
        <loader v-else />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Getter, Action } from 'vuex-class';
import { Review, User } from '@/api/models/community';
import IconType from '@/components/iconType';
import { userIsAdmin } from '@/api/models/community/helpers';

@Component({})
export default class ReviewComponent extends Vue {
  private IconType: typeof IconType = IconType;

  private loading = false;

  @Prop()
  private review!: Review;

  @Prop()
  private controls!: boolean;

  @Getter('user')
  private user!: User | null;

  @Action('community/approveReview')
  private approveReview: any;

  @Action('community/removeReview')
  private removeReview: any;

  private get canAdministrate(): boolean {
    return this.controls && userIsAdmin(this.user);
  }

  private async approve(): Promise<void> {
    this.loading = true;
    await this.approveReview({ id: this.review.id, router: this.$router, route: this.$route });
    this.loading = false;
  }

  private async remove(): Promise<void> {
    this.loading = true;
    await this.removeReview({ id: this.review.id, router: this.$router, route: this.$route });
    this.loading = false;
  }
}
</script>

<style lang="stylus" scoped>
.review
  margin $medium 0

.review-text
  position relative

  padding $medium
  margin-bottom $small

  border-radius $borderRadius
  theme(background-color, $panelHighlightBackground)
  transition all $animationTime

  &:after
    position absolute
    top 100%
    left $small

    content ''
    border solid $minor
    theme(border-top-color, $panelHighlightBackground)
    theme(border-left-color, $panelHighlightBackground)
    border-bottom-color transparent
    border-right-color transparent
    transition all $animationTime

.review-info
  display flex
  justify-content space-between

.review-controls
  secondary()
  display flex
  > *
    margin-left $small
</style>
