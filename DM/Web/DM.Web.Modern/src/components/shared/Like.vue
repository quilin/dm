<template>
  <a v-if="canInteract" class="like likeable"
     @click="$emit(userLiked ? 'unliked' : 'liked')" :class="{ active: userLiked }">
    <icon :font="IconType.Like" />
    <template v-if="entity.likes.length"> {{ entity.likes.length }}</template>
  </a>
  <span v-else class="like">
    <icon :font="IconType.Like" />
    <template v-if="entity.likes.length"> {{ entity.likes.length }}</template>
  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import IconType from '@/components/iconType';
import { Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import Likeable from '@/components/shared/likeable';

@Component({})
export default class Like extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private entity!: Likeable;

  @Getter('user')
  private user!: User;

  private get canInteract(): boolean {
    return this.user && this.entity.author.login !== this.user.login;
  }

  private get userLiked(): boolean {
    return this.entity.likes.some((liker: User) => liker.login === this.user.login);
  }
}
</script>

<style scoped lang="stylus">
.like
  opacity 0.5
  theme(color, $activeText)
  cursor default

  &.likeable
    cursor pointer

    &:hover, &.active
      opacity 1
</style>