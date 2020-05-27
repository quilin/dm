<template>
  <menu-block token="ModerationGames">
    <template v-slot:title>Требуют премодерации</template>
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
export default class ModerationGames extends Vue {
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
