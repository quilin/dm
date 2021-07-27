<template>
  <div>

    <paging v-if="users"
      :paging="users.paging"
      :to="{ name: 'community', params: $route.params }" />

    <div class="list">
      <div>#</div>
      <div>Логин</div>
      <div>Рейтинг</div>
      <div>В сети</div>
      <div>Имя</div>
      <div>Местоположение</div>
    </div>

    <loader v-if="!users" :big="true" />
    <template v-else-if="users.resources.length">
      <community-user v-for="(user, number) in users.resources" :key="user.login"
        :user="user"
        :number="number + (users.paging.size * (users.paging.current - 1)) + 1" />
    </template>

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';

import { User } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

import CommunityUser from './CommunityUser.vue';

@Component({
  components: {
    CommunityUser,
  },
})
export default class UsersList extends Vue {
  @Getter('community/users')
  private users!: ListEnvelope<User> | null;
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/community/Grid'

.list
  gridHead($communityGridTemplate)
  margin-top $medium
</style>
