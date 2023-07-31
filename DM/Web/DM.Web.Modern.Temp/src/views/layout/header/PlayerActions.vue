<script setup lang="ts">
import { IconType } from "@/components/icons/iconType";
import { useUserStore } from "@/stores";
import { storeToRefs } from "pinia";

const userStore = useUserStore();
const { unreadConversations: unread } = storeToRefs(userStore);
const { signOut } = userStore;
</script>

<template>
  <router-link :to="{ name: 'messenger' }" :class="{ unread }">
    <template v-if="unread">{{ unread }}</template>
    <the-icon
      :font="unread ? IconType.MessagesUnread : IconType.MessagesNoUnread"
    />
    Сообщения
  </router-link>
  |
  <a @click="signOut"><the-icon :font="IconType.Logout" /> Выйти</a>
</template>

<style scoped lang="sass"></style>
