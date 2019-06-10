<template>
  <div>
    <template v-if="canCreateTopic">
      <input type="button" value="Открыть тему" />
    </template>
    <template v-if="canMarkAsRead">
      <input type="button" value="Отметить прочитанным" />
    </template>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';

const namespace = 'forum';

@Component({})
export default class ForumActions extends Vue {
  @Getter('user')
  private user!: User | null;

  @Getter('selectedForum', { namespace })
  private selectedForum!: string;

  private get canCreateTopic(): boolean {
    return this.user !== null &&
      (this.selectedForum !== 'Новости проекта' ||
       this.user.roles.some((r: string) =>
        r === 'Admin' || r === 'SeniorModerator'));
  }

  private get canMarkAsRead(): boolean {
    return this.user !== null;
  }
}
</script>

<style lang="stylus">
</style>
