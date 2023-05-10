import { defineStore } from "pinia";
import { ref } from "vue";
import type { Forum, Topic } from "@/api/models/forum";
import type { User } from "@/api/models/community";
import type { ListEnvelope, PagingQuery } from "@/api/models/common";
import forumApi from "@/api/requests/forumApi";

export const useForumStore = defineStore("fora", () => {
  const fora = ref<Forum[] | null>(null);
  async function fetchFora() {
    const { data } = await forumApi.getFora();
    fora.value = data!.resources;
  }

  const news = ref<Topic[] | null>(null);
  async function fetchNews() {
    const { data } = await forumApi.getNews();
    news.value = data!.resources;
  }

  const selectedForumId = ref<string | null>(null);
  async function trySelectForum(forumId: string) {
    const { error } = await forumApi.getForum(forumId);
    if (error) return false;

    selectedForumId.value = forumId;
    return true;
  }

  const moderators = ref<User[] | null>(null);
  async function fetchModerators() {
    if (!selectedForumId.value) return;

    const { data } = await forumApi.getModerators(selectedForumId.value);
    if (data) moderators.value = data.resources;
  }

  const attachedTopics = ref<Topic[] | null>(null);
  const topics = ref<ListEnvelope<Topic> | null>(null);
  async function fetchTopics(number: number) {
    if (!selectedForumId.value) return;

    const query = { number } as PagingQuery;
    const [fetchedAttachedTopics, fetchedTopics] = await Promise.all([
      forumApi.getTopics(selectedForumId.value, query, true),
      forumApi.getTopics(selectedForumId.value, query, false),
    ]);

    attachedTopics.value = fetchedAttachedTopics.data!.resources;
    topics.value = fetchedTopics.data!;
  }

  return {
    fora,
    fetchFora,
    selectedForumId,
    trySelectForum,
    moderators,
    fetchModerators,
    attachedTopics,
    topics,
    fetchTopics,
    news,
    fetchNews,
  };
});
