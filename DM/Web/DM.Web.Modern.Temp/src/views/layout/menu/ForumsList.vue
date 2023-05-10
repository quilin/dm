<template>
  <menu-block token="fora">
    <template #title>Форумы</template>
    <the-loader v-if="!store.fora" />
    <div
      v-else
      v-for="forum in store.fora"
      :key="forum.id"
      :class="{ selected: forum.id === store.selectedForumId }"
    >
      <router-link :to="{ name: 'forum', params: { id: forum.id, n: 1 } }">
        {{ forum.id }}
        <template v-if="forum.unreadTopicsCount">
          <the-icon :font="IconType.CommentsUnread" />
          {{ forum.unreadTopicsCount }}
        </template>
      </router-link>
    </div>
  </menu-block>
</template>

<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import TheLoader from "@/components/TheLoader.vue";
import { useForumStore, useUserStore } from "@/stores";
import { onMounted, watch } from "vue";
import { IconType } from "@/components/icons/iconType";

const store = useForumStore();
const userStore = useUserStore();

onMounted(() => store.fetchFora());
watch(
  () => userStore.user,
  () => store.fetchFora()
);
</script>

<style scoped lang="sass">
.selected a
  font-weight: bold
  +theme(color, $text)
</style>
