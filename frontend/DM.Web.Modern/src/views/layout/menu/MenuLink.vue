<template>
  <div class="menu-game-item">
    <router-link :to="{ name: 'game', params: { id: game.id } }">
      {{ game.title }}
    </router-link>
    <span v-if="counters">
      <router-link :to="{ name: 'game-first-unread-post', params: { id: game.id } }">
        <icon :font="game.unreadPostsCount ? IconType.PostsUnread : IconType.PostsNoUnread" />
        <template v-if="game.unreadPostsCount">{{ game.unreadPostsCount }}</template>
      </router-link>
      <span class="menu-game-item-separator">|</span>      
      <router-link :to="{ name: 'game-comments', params: { id: game.id } }">
        <icon :font="game.unreadCommentsCount ? IconType.CommentsUnread : IconType.CommentsNoUnread" />
        <template v-if="game.unreadCommentsCount">{{ game.unreadCommentsCount }}</template>
      </router-link>
      <template v-if="game.unreadCharactersCount">
        <span class="menu-game-item-separator">|</span>
        <router-link :to="{ name: 'game-characters', params: { id: game.id } }">
          <icon :font="IconType.User" />
          {{ game.unreadCharactersCount }}
        </router-link>
      </template>
    </span>
  </div>
</template>

<script lang="ts">
import { Prop, Component, Vue } from 'vue-property-decorator';

import { Game } from '@/api/models/gaming/games';
import IconType from '@/components/iconType';

@Component({})
export default class MenuLink extends Vue {
  private IconType: typeof IconType = IconType;

  @Prop()
  private game!: Game;

  @Prop()
  private counters!: boolean;
}
</script>

<style scoped lang="stylus">
.menu-game-item
  display flex
  justify-content space-between

  & a.router-link-active
    theme(color, $text)

.menu-game-item-separator
  display inline-block
  margin 0 $tiny
  vertical-align top
  theme(color, $secondaryText)
  cursor default
</style>