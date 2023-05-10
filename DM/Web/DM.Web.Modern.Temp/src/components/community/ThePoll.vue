<template>
  <div class="poll">
    <div class="poll-name">
      {{ props.poll.title }}
      <secondary-text v-if="closed">Голосование окончено</secondary-text>
    </div>
    <progress-bar
      v-for="option in props.poll.options"
      :key="option.id"
      :current="option.votesCount"
      :goal="totalVotes || 1"
    >
      <the-icon v-if="option.voted" :font="IconType.Tick" />
      {{ option.text }}&nbsp;&ndash;&nbsp;{{ option.votesCount }}
      <a
        v-if="!closed && userStore.user && !voted"
        @click="participateInPoll(option.id)"
        class="poll-option-vote"
      />
    </progress-bar>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import dayjs from "dayjs";
import type { Poll } from "@/api/models/community";
import ProgressBar from "@/components/ProgressBar.vue";
import { useUserStore, useCommunityStore } from "@/stores";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import { IconType } from "@/components/icons/iconType";

const props = defineProps<{ poll: Poll }>();
const userStore = useUserStore();
const { vote } = useCommunityStore();

const closed = computed(() => dayjs(props.poll.ends).isBefore(dayjs()));

const totalVotes = computed(() =>
  props.poll.options.reduce((sum, option) => sum + option.votesCount, 0)
);
const voted = computed(() => props.poll.options.some((option) => option.voted));

const participateInPoll = async (optionId: string) => {
  await vote(props.poll.id!, optionId);
};
</script>

<style scoped lang="sass">
.poll
  margin: $small 0 $big
  max-width: $grid-step * 61

  &:last-child
    margin-bottom: 0

.poll-name
  margin: $small 0

.poll-option-vote
  display: block
  position: absolute
  top: 0
  left: 0
  right: 0
  bottom: 0
</style>
