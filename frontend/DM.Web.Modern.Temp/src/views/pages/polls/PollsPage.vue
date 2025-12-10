<script setup lang="ts">
import { useFetchData } from "@/composables/useFetchData";
import { usePollsStore } from "@/stores/polls";
import router, { extractNumberParam } from "@/router";
import { useRoute } from "vue-router";
import { IconType } from "@/components/ui-kit/iconType";
import { useModal } from "vue-final-modal";
import { computed } from "vue";
import { storeToRefs } from "pinia";
import { useUserStore } from "@/stores";
import { userIsHighAuthority } from "@/api/models/community/helpers";
import CreatePoll from "@/views/pages/polls/CreatePoll.vue";

const route = useRoute();
const { fetchPolls } = usePollsStore();
const { user } = storeToRefs(useUserStore());

useFetchData(
  () => fetchPolls(extractNumberParam(route.params.n), false),
  [
    {
      param: (p) => p.n,
      callback: (p) => fetchPolls(extractNumberParam(p), false),
    },
  ],
);

const canCreatePoll = computed(() => userIsHighAuthority(user.value));
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
