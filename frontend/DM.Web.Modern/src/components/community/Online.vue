<template>
  <span :class="{online}">
    <template v-if="online">online</template>
    <human-timespan v-else-if="detailed" :date="user.online" />
    <span v-else class="offline">offline</span>
  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { User } from '@/api/models/community';
import moment from 'moment';

@Component({})
export default class Online extends Vue {
  @Prop()
  private user!: User;

  @Prop()
  private detailed?: boolean;

  private get online(): boolean {
    return moment(moment.now()).diff(this.user.online, 'minutes') < 5;
  }
}
</script>

<style scoped lang="stylus">
.online
  theme(color, $positiveText)

.offline
  theme(color, $secondaryText)
</style>