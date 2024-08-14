<template>
  <menu-block v-if="user" token="OwnGames">
    <template v-slot:title>Мои игры</template>

    <loader v-if="!ownGames" />
    <menu-link v-else v-for="game in ownGames" :key="game.id" :game="game" :counters="true" />

    <router-link :to="{name: 'games'}" class="menu-rest-link">
      Все активные игры
      <icon :font="IconType.Forward" />
    </router-link>
  </menu-block>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import { User } from '@/api/models/community';
import { Game } from '@/api/models/gaming/games';
import IconType from '@/components/iconType';
import MenuBlock from '../MenuBlock.vue';
import MenuLink from './MenuLink.vue';

@Component({
  components: {
    MenuBlock,
    MenuLink,
  },
})
export default class OwnGames extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('user')
  private user!: User;

  @Getter('gaming/ownGames')
  private ownGames!: Game[];

  private get activeGameRoute(): boolean {
    const name = this.$route.name;
    return name === 'game';
  }

  @Action('gaming/fetchOwnGames')
  private fetchOwnGames: any;

  @Watch('user')
  private onUserChange() {
    this.fetchOwnGames();
  }

  private mounted() {
    this.fetchOwnGames();
  }
}
</script>

<style scoped lang="stylus">
.menu-game-item
  display flex
  justify-content space-between

.menu-rest-link
  font-weight bold
</style>
