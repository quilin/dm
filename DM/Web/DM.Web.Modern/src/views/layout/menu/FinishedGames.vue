<template>
  <menu-block token="FinishedGames">
    <template v-slot:title>Завершённые игры</template>
    <router-link :to="{name: 'games'}" class="menu-rest-link">
      Все завершённые игры
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

@Component({
  components: {
    MenuBlock,
  },
})
export default class FinishedGames extends Vue {
  private IconType: typeof IconType = IconType;
}
</script>

<style scoped lang="stylus">
.menu-game-item
  display flex
  justify-content space-between

.menu-rest-link
  font-weight bold
</style>