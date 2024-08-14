<template>
  <menu-block token="PopularGames">
    <template v-slot:title>Самые читаемые игры</template>

    <loader v-if="!popularGames" />
    <menu-link v-else v-for="game in popularGames" :key="game.id" :game="game" :counters="true" />
  </menu-block>
</template>

<script lang="ts">

import { Component, Vue } from 'vue-property-decorator';
import { Game } from '@/api/models/gaming';
import MenuBlock from '@/views/layout/MenuBlock.vue';
import MenuLink from '@/views/layout/menu/MenuLink.vue';
import { Action, Getter } from 'vuex-class';

@Component({
  components: {
    MenuBlock,
    MenuLink,
  },
})
export default class PopularGames extends Vue {
  @Getter('gaming/popularGames')
  private popularGames!: Game[];

  @Action('gaming/fetchPopularGames')
  private fetchPopularGames: any;

  private mounted() {
    this.fetchPopularGames();
  }
}
</script>
