<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { usePollsStore } from "@/stores/polls";
import { onMounted } from "vue";
import ThePoll from "@/views/pages/polls/ThePoll.vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";
import { useI18n } from "vue-i18n";
import messages from "@/views/layout/sidebar/ActivePolls.i18n";

const store = usePollsStore();
const { t } = useI18n({ messages });

onMounted(() => store.fetchActivePolls());
</script>

<template>
  <menu-block token="OpenPolls">
    <template #title>{{ t("title") }}</template>
    <dm-loader v-if="store.activePolls === null" />
    <secondary-text v-else-if="!store.activePolls.length">{{
      t("empty")
    }}</secondary-text>
    <the-poll
      v-else
      v-for="poll in store.activePolls"
      :key="poll.id"
      :poll="poll"
    />
    <div>
      <router-link class="forward" :to="{ name: 'polls' }">
        {{ t("all") }}
        <dm-icon :font="IconType.Forward" />
      </router-link>
    </div>
  </menu-block>
</template>

<style scoped lang="sass">
.forward
  font-weight: bold
</style>
