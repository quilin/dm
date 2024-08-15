<script setup lang="ts">
import ThePaging from "@/components/ThePaging.vue";
import { storeToRefs } from "pinia";
import UserRating from "@/components/community/UserRating.vue";
import UserOnline from "@/components/community/UserOnline.vue";
import { useCommunityStore } from "@/stores/community";
import { useRoute } from "vue-router";

const { users } = storeToRefs(useCommunityStore());
const route = useRoute();
</script>

<template>
  <the-paging
    v-if="users"
    :paging="users.paging!"
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

  <the-loader v-if="!users" :big="true" />
  <secondary-text v-else-if="!users.resources.length" class="users-list-none"
    >Пользователей нет...</secondary-text
  >
  <div
    class="users-list-row"
    v-else
    v-for="(user, number) in users.resources"
    :key="user.login"
  >
    <span class="number">{{
      number + users.paging!.size * (users.paging!.current - 1) + 1
    }}</span>
    <user-link :user="user" />
    <user-rating :user="user" />
    <user-online :user="user" :detailed="true" />
    <span>{{ user.name }}</span>
    <span>{{ user.location }}</span>
  </div>
</template>

<style scoped lang="sass">
@import "@/assets/styles/Grid"

$grid-template: [number] 6% [login] auto [rating] 12% [online] 8% [name] 25% [location] 25%

.users-list-header
  +grid-head($grid-template)

.users-list-row
  +grid($grid-template)

.users-list-none
  margin: $medium 0
  text-align: center
</style>
