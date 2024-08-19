<script setup lang="ts">
import type { User } from "@/api/models/community";

defineProps<{
  comment: string;
  author: User;
  created: string;
  updated?: string | null;
}>();
</script>

<template>
  <div class="comment">
    <router-link
      :to="{ name: 'profile', params: { login: author.login } }"
      class="comment_author"
    >
      <div
        class="comment_author_picture"
        :style="{ backgroundImage: `url(${author.mediumPictureUrl})` }"
      />
      <div class="comment_author_info">
        <a>{{ author.login }}</a>
        <secondary-text>
          <human-timespan :date="created" /><template v-if="updated"
            >, изменен <human-timespan :date="updated"
          /></template>
        </secondary-text>
      </div>
    </router-link>
    <div class="comment_text" v-html="comment" />
  </div>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Layout"

.comment
  margin: $medium 0 $big

.comment_author
  display: flex
  margin-bottom: $small

.comment_author_picture
  margin-right: $small
  +square($big)
  border-radius: $big
  background-size: contain

.comment_text
  padding-left: $big + $small
</style>
