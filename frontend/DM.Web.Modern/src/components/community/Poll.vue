<template>
  <div class="poll">
    <div class="poll-name">
      {{poll.title}}
      <div v-if="closed" class="poll-closed">Голосование окончено</div>
    </div>
    <progress-bar v-for="option in poll.options" :key="option.id"
      :current="option.votesCount"
      :goal="totalVotes || 1">
      <icon v-if="option.voted" :font="IconType.Tick" />
      {{option.text}}&nbsp;&ndash;&nbsp;{{option.votesCount}}
      <a v-if="!closed && user && !voted" @click="vote({ router: $router, pollId: poll.id, optionId: option.id })" class="poll-option-vote" />
    </progress-bar>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Getter, Action } from 'vuex-class';
import moment from 'moment';

import { Poll, User } from '@/api/models/community';
import IconType from '@/components/iconType';
import ProgressBar from '@/components/ProgressBar.vue';

@Component({
  components: {
    ProgressBar,
  },
})
export default class PollComponent extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private poll!: Poll;

  @Getter('user')
  private user!: User | null;

  @Action('community/vote')
  private vote: any;

  private get closed(): boolean {
    return moment(this.poll.ends).isBefore(moment());
  }

  private get totalVotes(): number {
    return this.poll.options.reduce((sum, option) => sum + option.votesCount, 0);
  }

  private get voted(): boolean {
    return this.poll.options.some(option => option.voted);
  }
}
</script>

<style scoped lang="stylus">
.poll
  margin $small 0 $big
  max-width $gridStep * 61

  &:last-child
    margin-bottom 0

.poll-name
  margin $small 0

.poll-closed
  secondary()

.poll-option-vote
  display block
  position absolute
  top 0
  left 0
  right 0
  bottom 0
</style>
