<template>
  <div>
    <menu-block v-if="true" token="MyGames">
      <template v-slot:title>Мои игры</template>
      Добавить условие про аутентификацию
    </menu-block>
    <menu-block v-else token="Active">
      <template v-slot:title>Активные игры</template>
      Все активные игры
    </menu-block>
    <menu-block token="Requirement">
      <template v-slot:title>Набор игроков</template>
      Все игры с открытым набором
    </menu-block>
    <menu-block token="Finished">
      <template v-slot:title>Завершенные игры</template>
      Все завершенные игры
    </menu-block>
    <menu-block token="Forum">
      <template v-slot:title>Форумы</template>
      <div v-for="forum in fora" :key="forum.id" class="menu-item" :class="{ selected: forum.id === selectedForum }">
        <router-link :to="{name: 'forum', params: {id: forum.id}}">
          {{forum.id}}
          <icon v-if="forum.unreadTopicsCount" :font="IconType.CommentsUnread" />
        </router-link>
      </div>
    </menu-block>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import { Forum } from '@/api/models/forum';
import IconType from '@/components/iconType';
import MenuBlock from './MenuBlock.vue';

@Component({
  components: {
    MenuBlock,
  },
})
export default class GeneralMenu extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('fora', { namespace: 'forum' })
  private fora!: Forum[];

  @Getter('selectedForum', { namespace: 'forum' })
  private selectedForum!: string | null;

  @Action('fetchFora', { namespace: 'forum' })
  private fetchFora: any;

  private mounted() {
    this.fetchFora();
  }
}
</script>

<style scoped lang="stylus">
.menu-item
  margin $tiny 0
  &.selected
    font-weight bold
</style>
