<template>
  <div>
    <div class="page-title">Опросы</div>
    <create-poll v-if="canCreatePoll" />
    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import CreatePoll from '@/views/pages/polls/CreatePoll.vue';
import { userIsHighAuthority } from '@/api/models/community/helpers';

@Component({
  components: {CreatePoll}
})
export default class Polls extends Vue {
  @Getter('user')
  private user!: User | null;

  private get canCreatePoll(): boolean {
    return userIsHighAuthority(this.user);
  }

  @Action('community/fetchPolls')
  private fetchPolls: any;

  @Watch('$route')
  private onRouteChange() {
    this.fetchData();
  }

  private mounted() {
    this.fetchData();
  }

  private fetchData() {
    const { n } = this.$route.params;
    this.fetchPolls({ number: n });
  }
}
</script>

<style lang="stylus">
</style>