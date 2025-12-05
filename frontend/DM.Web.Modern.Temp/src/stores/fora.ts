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
import type { Post } from "@/api/models";

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
  async function markAllTopicsAsRead() {
    await forumApi.markAllTopicsAsRead(selectedForum.value!.id);
    topics.value?.resources.forEach(
      (topic) => ((topic.unreadCommentsCount as number) = 0),
    );
    attachedTopics.value?.forEach(
      (topic) => ((topic.unreadCommentsCount as number) = 0),
    );
    await fetchFora();
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

  async function createTopic(topic: Post<Topic>) {
    const { data } = await forumApi.createTopic(selectedForum.value!.id, topic);
    return data!.resource;
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
  async function fetchComments(number?: number) {
    comments.value = null;
    if (!selectedTopic.value) return;

    const { data } = await forumApi.getComments(selectedTopic.value.id, {
      number,
    });
    await forumApi.markTopicAsRead(selectedTopic.value!.id);
    comments.value = data;

    await fetchFora();
  }
  async function reloadComments() {
    if (!selectedTopic.value || !comments.value) return;
    const { data } = await forumApi.getComments(selectedTopic.value.id, {
      number: comments.value.paging!.number,
    });
    comments.value = data;
  }
  async function createComment(comment: Post<Comment>) {
    const result = await forumApi.createComment(
      selectedTopic.value!.id,
      comment,
    );

    if (result.data) {
      await forumApi.markTopicAsRead(selectedTopic.value!.id);
      await fetchFora();
    }
    return result;
  }

  return {
    fora,
    fetchFora,
    selectedForum,
    trySelectForum,
    markAllTopicsAsRead,
    moderators,
    fetchModerators,
    attachedTopics,
    topics,
    fetchTopics,
    createTopic,
    news,
    fetchNews,
    trySelectTopic,
    selectedTopic,
    fetchComments,
    reloadComments,
    createComment,
    comments,
  };
});
