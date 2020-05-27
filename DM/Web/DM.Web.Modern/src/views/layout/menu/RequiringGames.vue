<template>
  <menu-block token="RequiringGames">
    <template v-slot:title>Идёт набор</template>
    <router-link :to="{name: 'games'}" class="menu-rest-link">
      Все игры с открытым набором
      <icon :font="IconType.Forward" />
    </router-link>
  </menu-block>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import IconType from '@/components/iconType';
import MenuBlock from '../MenuBlock.vue';

@Component({
  components: {
    MenuBlock,
  },
})
export default class RequiringGames extends Vue {
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
