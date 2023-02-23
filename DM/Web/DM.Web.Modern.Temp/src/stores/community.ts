import { defineStore } from "pinia";
import communityApi from "@/api/requests/communityApi";
import type { Poll } from "@/api/models/community";
import { ref } from "vue";
import type { ListEnvelope } from "@/api/models/common";

export const useCommunityStore = defineStore("community", () => {
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

  return { vote };
});
