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
      Форумы!!!
    </menu-block>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import MenuBlock from './MenuBlock.vue';

@Component({
  components: {
    MenuBlock,
  },
})
export default class GeneralMenu extends Vue {

}
</script>
