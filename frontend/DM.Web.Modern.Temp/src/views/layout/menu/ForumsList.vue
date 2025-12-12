<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { useForumStore, useUserStore } from "@/stores";
import { onMounted, watch } from "vue";
import { IconType } from "@/components/ui-kit/iconType";
import { useRoute } from "vue-router";
import { computed } from "vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";

const route = useRoute();
const forumStore = useForumStore();
const userStore = useUserStore();

const isForumRoute = computed(
  () => route.name === "forum" || route.name === "topic",
);

// TODO: fix double invocation
onMounted(forumStore.fetchFora);
watch(() => userStore.user, forumStore.fetchFora, { flush: "post" });
</script>

<template>
  <menu-block token="fora">
    <template #title>Форумы</template>
    <dm-loader v-if="forumStore.fora === null" />
    <div
      v-else
      v-for="forum in forumStore.fora"
      :key="forum.id"
      :class="{
        selected: isForumRoute && forum.id === forumStore.selectedForum?.id,
      }"
    >
      <router-link :to="{ name: 'forum', params: { id: forum.id } }">
        {{ forum.id }}
        <template v-if="forum.unreadTopicsCount">
          <dm-icon :font="IconType.CommentsUnread" />
          {{ forum.unreadTopicsCount }}
        </template>
      </router-link>
    </div>
  </menu-block>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Themes"

.selected a
  font-weight: bold
  +Themes.theme(color, Themes.$text)
</style>
