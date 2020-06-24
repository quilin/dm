<template>
  <div class="community-info">
    <div class="page-title">Сообщество</div>
    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import { ListEnvelope } from '@/api/models/common';

@Component({})
export default class Community extends Vue {
  @Getter('community/users')
  private users!: ListEnvelope<User> | null;

  @Action('community/fetchUsers')
  private fetchUsers: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const { n } = this.$route.params;
    this.fetchUsers({ n });
  }
}
</script>