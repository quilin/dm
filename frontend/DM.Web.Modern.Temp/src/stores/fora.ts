import { defineStore } from "pinia";
import { ref } from "vue";
import type {
  Comment,
  Forum,
  ForumId,
  Topic,
  TopicId,
} from "@/api/models/forum";
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

  const selectedForum = ref<Forum | null>(null);
  async function trySelectForum(id: ForumId) {
    const localForum = fora.value?.find((f) => f.id === id);
    if (localForum) selectedForum.value = localForum;

    const { error, data } = await forumApi.getForum(id);
    if (error) return false;

    selectedForum.value = data!.resource;
    return true;
  }

  const moderators = ref<User[] | null>(null);
  async function fetchModerators() {
    if (!selectedForum.value) return;

    const { data } = await forumApi.getModerators(selectedForum.value!.id);
    if (data) moderators.value = data.resources;
  }

  const attachedTopics = ref<Topic[] | null>(null);
  const topics = ref<ListEnvelope<Topic> | null>(null);
  async function fetchTopics(number: number) {
    if (!selectedForum.value) return;

    const query = { number } as PagingQuery;
    const [fetchedAttachedTopics, fetchedTopics] = await Promise.all([
      forumApi.getTopics(selectedForum.value!.id, query, true),
      forumApi.getTopics(selectedForum.value!.id, query, false),
    ]);

    attachedTopics.value = fetchedAttachedTopics.data!.resources;
    topics.value = fetchedTopics.data!;
  }

  const selectedTopic = ref<Topic | null>(null);
  async function trySelectTopic(id: TopicId) {
    if (selectedTopic.value?.id !== id) selectedTopic.value = null;
    const { data } = await forumApi.getTopic(id);
    if (!data) return;

    const { resource: topic } = data;
    selectedTopic.value = topic;
    await trySelectForum(topic.forum.id);
  }

  const comments = ref<ListEnvelope<Comment> | null>(null);
  async function fetchComments(number: number) {
    comments.value = null;
    if (!selectedTopic.value) return;

    await new Promise((resolve) => setTimeout(resolve, 2000));
    const { data } = await forumApi.getComments(selectedTopic.value.id!, {
      number,
    } as PagingQuery);
    comments.value = data;
  }

  return {
    fora,
    fetchFora,
    selectedForum,
    trySelectForum,
    moderators,
    fetchModerators,
    attachedTopics,
    topics,
    fetchTopics,
    news,
    fetchNews,
    trySelectTopic,
    selectedTopic,
    fetchComments,
    comments,
  };
});
