<template>
  <div>

    <paging v-if="polls"
      :paging="polls.paging"
      :to="{ name: 'polls', params: $route.params }" />

    <loader v-if="polls === null" :big="true" />

    <div v-else-if="polls.resources.length">
      <poll-component v-for="poll in polls.resources" :key="poll.id" :poll="poll" />
    </div>

    <div v-else>Пока нет опросов</div>

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';
import { ListEnvelope } from '../../../api/models/common';
import { Poll } from '../../../api/models/community';
import PollComponent from '../../../components/community/Poll.vue';

@Component({
  components: {
    PollComponent,
  }
})
export default class PollsList extends Vue {
  @Getter('community/polls')
  private polls!: ListEnvelope<Poll> | null;
}
</script>

<style lang="stylus">
.poll-container
  margin-bottom $big
</style>