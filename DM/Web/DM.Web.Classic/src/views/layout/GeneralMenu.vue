<template>
  <div class="content-menu">
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
      <div v-for="forum in fora" :key="forum.id" class="menu-item">
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
import { mapActions, mapGetters } from 'vuex';

import IconType from '@/components/iconType';
import MenuBlock from './MenuBlock.vue';

@Component({
  components: {
    MenuBlock,
  },
  computed: {
    ...mapGetters(['fora']),
  },
})
export default class GeneralMenu extends Vue {
  private IconType: typeof IconType = IconType;

  private mounted() {
    this.$store.dispatch('fetchFora');
  }
}
</script>

<style scoped lang="stylus">
.menu-item
  margin $tiny 0
</style>
