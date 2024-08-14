<template>
  <span :title="localMoment">{{humanDate}}</span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import moment from 'moment';

@Component({})
export default class HumanTimespan extends Vue {
  @Prop()
  private date!: string;

  private get humanDate(): string {
    return moment(this.date).fromNow();
  }

  private get localMoment(): string {
    return moment(this.date).format('DD MMMM YYYY HH:mm:SS');
  }
}
</script>
