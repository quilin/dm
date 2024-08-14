<template>
  <div class="container">
    <span class="title">{{title}}</span>
    <span class="value"><slot /></span>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component({})
export default class ProfileStat extends Vue {
  @Prop()
  private title!: string;
}
</script>

<style scoped lang="stylus">
.container
  display flex
  margin $medium 0

.title
  width $gridStep * 20
  theme(color, $secondaryText)
</style>