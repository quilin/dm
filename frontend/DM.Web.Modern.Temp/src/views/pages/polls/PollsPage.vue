<script setup lang="ts">
import { useFetchData } from "@/composables/useFetchData";
import router, { extractNumberParam } from "@/router";
import { useRoute } from "vue-router";
import { IconType } from "@/components/ui-kit/iconType";
import { useModal } from "vue-final-modal";
import { computed } from "vue";
import { useUserStore, usePollsStore } from "@/stores";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import CreatePoll from "@/views/pages/polls/CreatePoll.vue";
import DmButton from "@/components/ui-kit/DmButton.vue";
import PageTitle from "@/components/layout/PageTitle.vue";

const route = useRoute();
const pollsStore = usePollsStore();
const userStore = useUserStore();

useFetchData(
  () => pollsStore.fetchPolls(extractNumberParam(route.params.n), false),
  [
    {
      param: (p) => p.n,
      callback: (p) => pollsStore.fetchPolls(extractNumberParam(p), false),
    },
  ],
);

const canCreatePoll = computed(() => userIsHighAuthority(userStore.user));
const { open: openCreatePoll, close: closeCreatePoll } = useModal({
  component: CreatePoll,
  attrs: {
    onCancelled: () => closeCreatePoll(),
    onCreated: () => {
      closeCreatePoll();
      router.push({ name: "polls" });
    },
  },
});
</script>

<template>
  <page-title>Опросы</page-title>
  <dm-button
    label="Новый опрос"
    :icon="IconType.Add"
    v-if="canCreatePoll"
    @click="openCreatePoll"
  />
  <router-view />
</template>
