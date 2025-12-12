<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { useUserStore } from "@/stores";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import { useI18n } from "vue-i18n";
import messages from "./PlayerActions.i18n";

const { t } = useI18n({ messages });
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
    {{ t("messages") }}
  </router-link>
  |
  <a @click="store.signOut"
    ><dm-icon :font="IconType.Logout" /> {{ t("signOut") }}</a
  >
</template>

<style scoped lang="sass"></style>
