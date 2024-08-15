<script setup lang="ts">
import { useForumStore } from "@/stores";
import { storeToRefs } from "pinia";
import ThePaging from "@/components/ThePaging.vue";
import { useRoute } from "vue-router";
import TheComment from "@/components/comments/TheComment.vue";

const route = useRoute();
const { comments } = storeToRefs(useForumStore());
</script>

<template>
  <the-paging
    v-if="comments"
    :paging="comments.paging!"
    :to="{ name: 'topic', params: route.params }"
  />
  <the-loader v-if="!comments" :big="true" />
  <secondary-text v-else-if="!comments.resources.length" class="comments-none"
    >Комментариев пока нет...</secondary-text
  >
  <the-comment
    v-else
    v-for="comment in comments.resources"
    :key="comment.id"
    :author="comment.author"
    :created="comment.created"
    :updated="comment.updated"
    :comment="comment.text"
  />
</template>

<style scoped lang="sass">
.comments-none
  text-align: center
</style>
