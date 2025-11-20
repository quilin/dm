<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { useForumStore, useUserStore } from "@/stores";
import { onMounted, watch } from "vue";
import { IconType } from "@/components/ui-kit/iconType";
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

// TODO: fix double invocation
onMounted(fetchFora);
watch(() => user.value, fetchFora, { flush: "post" });
</script>

<template>
  <menu-block token="fora">
    <template #title>Форумы</template>
    <dm-loader v-if="!fora" />
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
