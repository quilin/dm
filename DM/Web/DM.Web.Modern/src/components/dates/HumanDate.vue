<template>
  <span>{{humanDate}}</span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import moment from 'moment';

@Component({})
export default class HumanDate extends Vue {
  @Prop()
  private date!: string;

  @Prop()
  private format!: string;

  private get humanDate(): string {
    return moment(this.date).format(this.format);
  }
}
</script>
