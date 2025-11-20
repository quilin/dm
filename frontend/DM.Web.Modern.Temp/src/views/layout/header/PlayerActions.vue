<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore } from "@/stores";
import { storeToRefs } from "pinia";

const userStore = useUserStore();
const { unreadConversations: unread } = storeToRefs(userStore);
const { signOut } = userStore;
</script>

<template>
  <router-link :to="{ name: 'messenger' }" :class="{ unread }">
    <template v-if="unread">{{ unread }}</template>
    <dm-icon
      :font="unread ? IconType.MessagesUnread : IconType.MessagesNoUnread"
    />
    Сообщения
  </router-link>
  |
  <a @click="signOut"><dm-icon :font="IconType.Logout" /> Выйти</a>
</template>

<style scoped lang="sass"></style>
