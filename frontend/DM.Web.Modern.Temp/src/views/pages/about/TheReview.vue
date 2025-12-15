<script setup lang="ts">
import type { Review } from "@/api/models/community";
import { IconType } from "@/components/ui-kit/iconType";
import { useReviewStore, useUserStore } from "@/stores";
import { computed, ref } from "vue";
import { userIsAdmin } from "@/api/models/community/helpers";
import { useRouter } from "vue-router";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import UserLink from "@/components/community/UserLink.vue";
import messages from "@/views/pages/about/TheReview.i18n";
import { useI18n } from "vue-i18n";

const props = defineProps<{
  review: Review;
  controls: boolean;
}>();
const userStore = useUserStore();
const communityStore = useReviewStore();
const { t } = useI18n({ messages });

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
        <span v-if="!review.approved">{{ t("checkPending") }}</span>
        <template v-if="!loading">
          <a v-if="!review.approved" @click="approve">
            <dm-icon :font="IconType.Tick" />
            {{ t("approve") }}</a
          >
          <a @click="remove">
            <dm-icon :font="IconType.Close" />
            {{ t("decline") }}</a
          >
        </template>
        <dm-loader v-else />
      </secondary-text>
    </div>
  </article>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.review
  margin: Variables.$medium 0

.review-text
  position: relative

  padding: Variables.$medium
  margin-bottom: Variables.$small

  border-radius: Variables.$border-radius
  +Themes.theme(background-color, Themes.$panel-background-highlight)

  &:after
    position: absolute
    top: 100%
    left: Variables.$small

    content: ''
    border: solid Variables.$minor transparent
    +Themes.theme(border-top-color, Themes.$panel-background-highlight)
    +Themes.theme(border-left-color, Themes.$panel-background-highlight)

.review-info
  display: flex
  justify-content: space-between

.review-controls
  display: flex
  & > *
    margin-left: Variables.$small
</style>
