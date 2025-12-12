<script setup lang="ts">
import MenuBlock from "@/views/layout/MenuBlock.vue";
import { usePollsStore } from "@/stores/polls";
import { onMounted } from "vue";
import ThePoll from "@/views/pages/polls/ThePoll.vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import SecondaryText from "@/components/layout/SecondaryText.vue";
import DmLoader from "@/components/ui-kit/DmLoader.vue";

const store = usePollsStore();

onMounted(() => store.fetchActivePolls());
</script>

<template>
  <menu-block token="OpenPolls">
    <template #title>Опросы</template>
    <dm-loader v-if="store.activePolls === null" />
    <secondary-text v-else-if="!store.activePolls.length"
      >Нет активных опросов</secondary-text
    >
    <the-poll
      v-else
      v-for="poll in store.activePolls"
      :key="poll.id"
      :poll="poll"
    />
    <div>
      <router-link class="forward" :to="{ name: 'polls' }">
        К старым опросам
        <dm-icon :font="IconType.Forward" />
      </router-link>
    </div>
  </menu-block>
</template>

<style scoped lang="sass">
.forward
  font-weight: bold
</style>
