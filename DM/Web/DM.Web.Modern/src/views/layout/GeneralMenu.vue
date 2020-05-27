<template>
  <div>
    <moderation-games v-if="user && user.roles.some(r => r ==='Administrator' || r === 'SeniorModerator')" />
    <own-games v-if="user" />
    <active-games v-else />

    <requiring-games />

    <finished-games />

    <fora />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';

import { User } from '@/api/models/community';
import IconType from '@/components/iconType';

import ModerationGames from './menu/ModerationGames.vue';
import OwnGames from './menu/OwnGames.vue';
import ActiveGames from './menu/ActiveGames.vue';
import RequiringGames from './menu/RequiringGames.vue';
import FinishedGames from './menu/FinishedGames.vue';
import Fora from './menu/Fora.vue';

@Component({
  components: {
    ModerationGames,
    OwnGames,
    ActiveGames,
    RequiringGames,
    FinishedGames,
    Fora,
  },
})
export default class GeneralMenu extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('user')
  private user!: User;
}
</script>

<style scoped lang="stylus">
.menu-game-item
  display flex
  justify-content space-between

.menu-rest-link
  font-weight bold
</style>
