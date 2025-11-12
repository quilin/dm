<script setup lang="ts">
import type { Poll, PollOptionId } from "@/api/models/community";
import ProgressBar from "@/components/ProgressBar.vue";
import { computed } from "vue";
import dayjs from "dayjs";
import { storeToRefs } from "pinia";
import { useUserStore } from "@/stores";
import { usePollsStore } from "@/stores/polls";

const { user } = storeToRefs(useUserStore());
const { vote } = usePollsStore();

const props = defineProps<{ poll: Poll }>();
const closed = computed(() => dayjs(props.poll.ends).isBefore(dayjs()));
const totalVotes = computed(() =>
  props.poll.options.reduce((sum, option) => sum + option.votesCount, 0),
);
const voted = computed(() => props.poll.options.some((option) => option.voted));

async function voteForOption(optionId: PollOptionId) {
  await vote(props.poll.id!, optionId);
}
</script>
<template>
  <div class="poll">
    <div class="poll-title">
      {{ poll.title }}<br />
      <secondary-text v-if="closed">Голосование окончено</secondary-text>
    </div>
    <progress-bar
      v-for="option in poll.options"
      :key="option.id"
      :current="option.votesCount"
      :goal="totalVotes || 1"
    >
      <div class="poll-option">
        <span>{{ option.text }}</span>
        <span>{{ option.votesCount }}</span>
      </div>
      <a
        v-if="!closed && user && !voted"
        @click="voteForOption(option.id)"
        class="poll-option-vote"
      />
    </progress-bar>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

.poll
  margin: Variables.$small 0
  max-width: Variables.$grid-step * 61
  &:last-child
    margin-bottom: 0

.poll-name
  margin: Variables.$small 0

.poll-option
  display: flex
  justify-content: space-between

.poll-option-vote
  display: block
  position: absolute
  top: 0
  left: 0
  right: 0
  bottom: 0
</style>
