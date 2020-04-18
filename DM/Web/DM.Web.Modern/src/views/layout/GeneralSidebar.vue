<template>
  <div>
    <menu-block token="OpenPolls">
      <template v-slot:title>Опросы</template>
      <loader v-if="polls === null" />
      <poll-component v-else v-for="poll in polls" :key="poll.id" :poll="poll" />
    </menu-block>
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import { Poll } from '@/api/models/community';
import IconType from '@/components/iconType';
import MenuBlock from './MenuBlock.vue';
import PollComponent from '@/components/Poll.vue';

@Component({
  components: {
    MenuBlock,
    PollComponent,
  },
})
export default class GeneralMenu extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('activePolls', { namespace: 'community' })
  private polls!: Poll[];

  @Action('fetchActivePolls', { namespace: 'community' })
  private fetchPolls: any;

  @Watch('user')
  private onUserChange() {
    this.fetchPolls();
  }

  private mounted() {
    this.fetchPolls();
  }
}
</script>

<style scoped lang="stylus">
.menu-item
  margin $tiny 0
  &.selected
    font-weight bold
</style>
