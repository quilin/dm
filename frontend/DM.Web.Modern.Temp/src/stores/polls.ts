import { defineStore } from "pinia";
import { ref } from "vue";
import type { Poll, PollId, PollOptionId } from "@/api/models/community";
import type { ListEnvelope } from "@/api/models/common";
import communityApi from "@/api/requests/communityApi";

export const usePollsStore = defineStore("polls", () => {
  const activePolls = ref<Poll[] | null>(null);
  async function fetchActivePolls() {
    const { data } = await communityApi.getPolls({ size: 3, skip: 0 }, true);
    activePolls.value = data!.resources;
  }
  const polls = ref<ListEnvelope<Poll> | null>(null);
  async function fetchPolls(number: number, onlyActive: boolean) {
    const { data } = await communityApi.getPolls({ number }, onlyActive);
    polls.value = data!;
  }

  function updatePoll(poll: Poll) {
    const matchingActivePoll = activePolls.value?.find((p) => p.id === poll.id);
    if (matchingActivePoll) Object.assign(matchingActivePoll, poll);

    const matchingPoll = polls.value?.resources.find((p) => p.id === poll.id);
    if (matchingPoll) Object.assign(matchingPoll, poll);
  }

  async function vote(pollId: PollId, optionId: PollOptionId) {
    const { data } = await communityApi.postPollVote(pollId, optionId);
    if (data) updatePoll(data.resource);
  }

  return { fetchActivePolls, activePolls, fetchPolls, polls, vote };
});
