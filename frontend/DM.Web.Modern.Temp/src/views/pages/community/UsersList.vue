<script setup lang="ts">
import UserRating from "@/components/community/UserRating.vue";
import UserOnline from "@/components/community/UserOnline.vue";
import { useCommunityStore } from "@/stores/community";
import { useRoute } from "vue-router";

const route = useRoute();
const store = useCommunityStore();
</script>

<template>
  <dm-paging
    v-if="store.users !== null"
    :paging="store.users.paging!"
    :to="{ name: 'community', params: route.params }"
  />

  <div class="users-list-header">
    <div>#</div>
    <div>Логин</div>
    <div>Рейтинг</div>
    <div>В сети</div>
    <div>Имя</div>
    <div>Местоположение</div>
  </div>

  <dm-loader v-if="store.users === null" :big="true" />
  <secondary-text
    v-else-if="!store.users.resources.length"
    class="users-list-none"
    >Пользователей нет...</secondary-text
  >
  <div
    class="users-list-row"
    v-else
    v-for="(user, number) in store.users.resources"
    :key="user.login"
  >
    <span class="number">{{
      number + store.users.paging!.size * (store.users.paging!.current - 1) + 1
    }}</span>
    <user-link :user="user" />
    <user-rating :user="user" />
    <user-online :user="user" />
    <span>{{ user.name }}</span>
    <span>{{ user.location }}</span>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Grid"

$grid-template: [number] 6% [login] auto [rating] 12% [online] 8% [name] 25% [location] 25%

.users-list-header
  +Grid.grid-head($grid-template)

.users-list-row
  +Grid.grid($grid-template)

.users-list-none
  margin: Variables.$medium 0
  text-align: center
</style>
