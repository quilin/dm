<template>
  <span :title="user.status">

    <router-link :to="{ name: 'profile', params: { login: user.login } }" class="user-link">
      <span :style="{ backgroundImage: user.smallPictureUrl ? `url(${user.smallPictureUrl})` : null }" class="user-logo" />
      {{user.login}}
    </router-link>

    <span v-if="badge" class="user-badge-container">
      [<span class="user-badge">{{badge}}</span>]
    </span>

  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { User } from '@/api/models/community';
import { userIsAdmin, userIsAuthority } from '@/api/models/community/helpers';

@Component({})
export default class UserLink extends Vue {
  @Prop()
  private user!: User;

  private get badge(): string | null {
    if (userIsAdmin(this.user)) {
      return 'A';
    }

    if (userIsAuthority(this.user)) {
      return 'M';
    }

    return null;
  }
}
</script>

<style lang="stylus" scoped>
.user-link
  white-space nowrap

.user-logo
  display inline-block

  width $medium
  height @width
  border-radius @width

  margin-right $minor
  background url('~@/assets/userpic.png') 0 0 no-repeat
  vertical-align text-bottom
  background-size cover

.user-badge-container
  secondary()

  .user-badge
    theme(color, $positiveText)
</style>