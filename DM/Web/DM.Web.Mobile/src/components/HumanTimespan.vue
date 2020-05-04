<template>
  <span>{{moment(time).fromNow()}}</span>
</template>

<script lang="ts">
  import { Component, Prop, Vue } from 'vue-property-decorator';
  import moment from 'moment';

  @Component({})
  export default class HumanTimespan extends Vue {
    private moment: any = moment;

    @Prop()
    private time!: string;
  }
</script>
