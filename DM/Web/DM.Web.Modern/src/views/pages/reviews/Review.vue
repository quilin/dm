<template>
  <div class="review">
    <div class="review-text" v-html="review.text" />
    <div class="review-info">
      <user-link :user="review.author" />
      <div v-if="canAdministrate">
        <template v-if="!loading">
          <a v-if="!review.approved" @click="approve"><icon :font="IconType.Tick" /></a>
          <a @click="remove"><icon :font="IconType.Close" /></a>
        </template>
        <loader v-else />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Getter, Action } from 'vuex-class';
import { Review, User, UserRole } from '@/api/models/community';
import IconType from '@/components/iconType';

@Component({})
export default class ReviewComponent extends Vue {
  private IconType: typeof IconType = IconType;

  private loading: boolean = false;

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
    return this.controls && this.user !== null &&
      this.user.roles.some((r: UserRole) => r === UserRole.Administrator);
  }

  private async approve(): Promise<void> {
    this.loading = true;
    await this.approveReview({ id: this.review.id, router: this.$router });
    this.loading = false;
  }

  private async remove(): Promise<void> {
    this.loading = true;
    await this.removeReview({ id: this.review.id, router: this.$router });
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
  theme(background-color, $panelBackground)

  &:after
    position absolute
    top 100%
    right $small

    content ''
    border solid $minor
    theme(border-top-color, $panelBackground)
    theme(border-right-color, $panelBackground)
    border-bottom-color transparent
    border-left-color transparent

.review-info
  display flex
  flex-direction row-reverse
  justify-content space-between
</style>