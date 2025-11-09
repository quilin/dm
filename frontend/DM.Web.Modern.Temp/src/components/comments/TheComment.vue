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
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

.comment
  margin: Variables.$medium 0 Variables.$big

.comment_author
  display: flex
  margin-bottom: Variables.$small

.comment_author_picture
  margin-right: Variables.$small
  +Layout.square(Variables.$big)
  border-radius: Variables.$big
  background-size: contain

.comment_text
  padding-left: Variables.$big + Variables.$small
</style>
