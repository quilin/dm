<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore } from "@/stores";

const store = useUserStore();
</script>

<template>
  <router-link
    :to="{ name: 'messenger' }"
    :class="{ unread: store.unreadConversations }"
  >
    <template v-if="store.unreadConversations">{{
      store.unreadConversations
    }}</template>
    <dm-icon
      :font="
        store.unreadConversations
          ? IconType.MessagesUnread
          : IconType.MessagesNoUnread
      "
    />
    Сообщения
  </router-link>
  |
  <a @click="store.signOut"><dm-icon :font="IconType.Logout" /> Выйти</a>
</template>

<style scoped lang="sass"></style>
