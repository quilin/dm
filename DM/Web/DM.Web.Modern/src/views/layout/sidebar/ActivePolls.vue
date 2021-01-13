<template>
  <div>
    <menu-block token="OpenPolls">
      <template v-slot:title>Опросы</template>

      <loader v-if="polls === null" />
      <div v-else-if="polls.length">
        <poll-component v-for="poll in polls" :key="poll.id" :poll="poll" />
      </div>
      <div v-else class="nothing">Нет активных опросов</div>

      <router-link :to="{ name: 'polls' }" class="rest-link">
        К прошедшим опросам
        <icon :font="IconType.Forward" />
      </router-link>
    </menu-block>
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Getter, Action } from 'vuex-class';

import { Poll } from '@/api/models/community';
import MenuBlock from '@/views/layout/MenuBlock.vue';
import PollComponent from '@/components/community/Poll.vue';
import IconType from '@/components/iconType';

@Component({
  components: {
    MenuBlock,
    PollComponent,
  },
})
export default class ActivePolls extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('community/activePolls')
  private polls!: Poll[];

  @Action('community/fetchActivePolls')
  private fetchPolls: any;

  @Watch('user')
  private onUserChange() {
    this.fetchPolls();
  }

  private mounted(): void {
    this.fetchPolls();
  }
}
</script>

<style scoped lang="stylus">
.nothing
  margin-bottom $minor
  secondary()

.rest-link
  font-weight bold
</style>