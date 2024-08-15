import { defineStore } from "pinia";
import { ref } from "vue";
import type { Poll } from "@/api/models/community";
import type { ListEnvelope, PagingQuery } from "@/api/models/common";
import communityApi from "@/api/requests/communityApi";

export const usePollsStore = defineStore("polls", () => {
  const activePolls = ref<Poll[] | null>(null);
  const polls = ref<ListEnvelope<Poll> | null>(null);

  function updatePoll(poll: Poll) {
    const matchingActivePoll = activePolls.value?.find((p) => p.id === poll.id);
    if (matchingActivePoll) Object.assign(matchingActivePoll, poll);

    const matchingPoll = polls.value?.resources.find((p) => p.id === poll.id);
    if (matchingPoll) Object.assign(matchingPoll, poll);
  }

  async function vote(pollId: string, optionId: string) {
    const { data } = await communityApi.postPollVote(pollId, optionId);
    if (data) updatePoll(data.resource);
  }

  async function fetchActivePolls() {
    const { resources } = await communityApi.getPolls(
      { size: 3, skip: 0 } as PagingQuery,
      true
    );
    activePolls.value = resources;
  }

  async function fetchPolls(number: number, onlyActive: boolean) {
    polls.value = await communityApi.getPolls(
      { number } as PagingQuery,
      onlyActive
    );
  }

  return { fetchActivePolls, activePolls, fetchPolls, polls, vote };
});
