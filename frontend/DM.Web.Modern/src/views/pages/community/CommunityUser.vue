<template>
  <div class="row">
    <span class="number">{{number}}</span>
    <user-link :user="user" />
    <rating :user="user" />
    <online :user="user" />
    <span>{{user.name}}</span>
    <span>{{user.location}}</span>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { User } from '@/api/models/community';

@Component({})
export default class CommunityUser extends Vue {
  @Prop()
  private user!: User;

  @Prop()
  private number!: number;
}
</script>

<style scoped lang="stylus">
@import '~@/views/pages/community/Grid'

.row
  grid($communityGridTemplate)
</style>