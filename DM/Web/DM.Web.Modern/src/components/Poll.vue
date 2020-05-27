<template>
  <div class="poll">
    <div class="poll-name">{{poll.title}}</div>
    <div v-if="closed" class="poll-closed">Голосование окончено</div>
    <div v-for="option in poll.options" :key="option.id" class="poll-option">
      <div class="poll-option-scale" :style="{width: `${option.votesCount / totalVotes}%`}"></div>
      <div class="poll-option-text">
        <icon v-if="option.voted" :font="IconType.Tick" />
        {{option.text}}&nbsp;&ndash;&nbsp;{{option.votesCount}}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

import { Poll } from '@/api/models/community';
import IconType from '@/components/iconType';

@Component({})
export default class PollComponent extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private poll!: Poll;

  private closed(): boolean {
    return new Date(this.poll.ends) < new Date();
  }

  private totalVotes(): number {
    return this.poll.options.reduce((sum, option) => sum + option.votesCount, 0);
  }
}
</script>

<style scoped lang="stylus">
.poll
  margin $small 0
</style>
