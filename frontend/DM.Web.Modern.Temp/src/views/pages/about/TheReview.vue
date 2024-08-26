<script setup lang="ts">
import type { Review } from "@/api/models/community";
import { IconType } from "@/components/icons/iconType";
import { useReviewStore, useUserStore } from "@/stores";
import { computed, ref } from "vue";
import { userIsAdmin } from "@/api/models/community/helpers";
import { useRouter } from "vue-router";

const props = defineProps<{
  review: Review;
  controls: boolean;
}>();
const userStore = useUserStore();
const communityStore = useReviewStore();

const canAdministrate = computed(
  () => props.controls && userIsAdmin(userStore.user),
);
const loading = ref(false);

async function approve() {
  loading.value = true;
  await communityStore.approveReview(props.review.id);
  loading.value = false;
}

async function remove() {
  loading.value = true;
  await communityStore.removeReview(props.review.id);
  const router = useRouter();
  await router.push(router.currentRoute.value);
  loading.value = false;
}
</script>

<template>
  <article class="review">
    <div class="review-text" v-html="review.text" />
    <div class="review-info">
      <user-link :user="review.author" />
      <secondary-text v-if="canAdministrate" class="review-controls">
        <span v-if="!review.approved">Ожидает проверки</span>
        <template v-if="!loading">
          <a v-if="!review.approved" @click="approve">
            <the-icon :font="IconType.Tick" />
            Принять</a
          >
          <a @click="remove">
            <the-icon :font="IconType.Close" />
            Отклонить</a
          >
        </template>
        <the-loader v-else />
      </secondary-text>
    </div>
  </article>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Themes"

.review
  margin: $medium 0

.review-text
  position: relative

  padding: $medium
  margin-bottom: $small

  border-radius: $border-radius
  +theme(background-color, $panel-background-highlight)

  &:after
    position: absolute
    top: 100%
    left: $small

    content: ''
    border: solid $minor transparent
    +theme(border-top-color, $panel-background-highlight)
    +theme(border-left-color, $panel-background-highlight)

.review-info
  display: flex
  justify-content: space-between

.review-controls
  display: flex
  & > *
    margin-left: $small
</style>
