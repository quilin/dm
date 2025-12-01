<script setup lang="ts">
import type { Comment } from "@/api/models/forum";

import UserLink from "@/components/community/UserLink.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import HumanTimespan from "@/components/dates/HumanTimespan.vue";
import DmMenu from "@/components/ui-kit/DmMenu.vue";
import { IconType } from "@/components/ui-kit/iconType";

const { comment } = defineProps<{ comment: Comment }>();
</script>

<template>
  <div class="container">
    <div class="wrapper">
      <router-link
        class="user-picture-container"
        :to="{ name: 'profile', params: { login: comment.author.login } }"
      >
        <div
          class="user-picture"
          :style="{
            backgroundImage:
              comment.author.mediumPictureUrl &&
              `url(${comment.author.mediumPictureUrl})`,
          }"
        ></div>
      </router-link>
      <div class="content">
        <div class="meta">
          <user-link :user="comment.author" hide-picture />,
          <secondary-text>
            <human-timespan :date="comment.created" />
            <template v-if="comment.updated">
              , (изменен
              <human-timespan :date="comment.updated" />)
            </template>
          </secondary-text>
        </div>
        <div v-html="comment.text" />
      </div>
      <dm-menu
        class="actions"
        :items="[
          { label: 'Редактировать', icon: IconType.Edit },
          { label: 'Удалить', icon: IconType.Remove },
        ]"
      />
    </div>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Layout"

.container
  margin: Variables.$small (-(Variables.$grid-step * 3))
  padding: Variables.$grid-step * 3
  border-radius: Variables.$border-radius

.wrapper
  display: flex
  position: relative

.actions
  position: absolute
  top: 0
  right: 0

.user-picture-container
  display: block
  height: Variables.$grid-step * 10
  flex-shrink: 0

.user-picture
  width: Variables.$grid-step * 10
  height: Variables.$grid-step * 10
  margin-right: Variables.$grid-step * 3
  background: url('@/assets/images/userpic.png') transparent left center no-repeat
  background-size: contain
  border-radius: Variables.$big

.content
  position: relative
  flex-grow: 1
</style>
