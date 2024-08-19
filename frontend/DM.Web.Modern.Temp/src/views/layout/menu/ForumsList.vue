<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { useForumStore, useUserStore } from "@/stores";
import { onMounted, watch } from "vue";
import { IconType } from "@/components/icons/iconType";
import { useRoute } from "vue-router";
import { computed } from "vue";
import { storeToRefs } from "pinia";

const store = useForumStore();
const { fora } = storeToRefs(store);
const { fetchFora } = store;
const { user } = storeToRefs(useUserStore());
const route = useRoute();

const isForumRoute = computed(
  () => route.name === "forum" || route.name === "topic",
);

onMounted(() => fetchFora());
watch(
  () => user,
  () => fetchFora(),
);
</script>

<template>
  <menu-block token="fora">
    <template #title>Форумы</template>
    <the-loader v-if="!fora" />
    <div
      v-else
      v-for="forum in fora!"
      :key="forum.id"
      :class="{
        selected: isForumRoute && forum.id === store.selectedForum?.id,
      }"
    >
      <router-link :to="{ name: 'forum', params: { id: forum.id } }">
        {{ forum.id }}
        <template v-if="forum.unreadTopicsCount">
          <the-icon :font="IconType.CommentsUnread" />
          {{ forum.unreadTopicsCount }}
        </template>
      </router-link>
    </div>
  </menu-block>
</template>

<style scoped lang="sass">
@import "src/assets/styles/Themes"

.selected a
  font-weight: bold
  +theme(color, $text)
</style>
