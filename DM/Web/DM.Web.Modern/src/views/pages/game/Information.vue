<template>
  <div>

    <div class="page-title">{{ game.title }}</div>
    <span v-if="game.released" class="game-disclaimer">
      <human-date :date="game.released" format="DD MMMM YYYY" />
      <template v-if="game.status in statuses"> &ndash;</template>
      <span v-if="game.status in statuses" :class="`game-status-${statuses[game.status].displayClass}`">
        {{ statuses[game.status].displayText }}
      </span>
    </span>

    <div class="form-field">
      <label>Мастер</label>
      <span><user-link :user="game.master" /></span>
    </div>
    <div v-if="game.assistant || game.pendingAssistant">
      <label>Ассистент</label>
      <span>
        <user-link :user="game.assistant || game.pendingAssistant" />
        <span class="game-disclaimer" v-if="game.pendingAssistant">(ждем подтверждения)</span>
      </span>
    </div>
    <div class="form-field">
      <label>Система</label>
      <span>{{ game.system }}</span>
    </div>
    <div class="form-field">
      <label>Сеттинг</label>
      <span>{{ game.setting }}</span>
    </div>
    <div class="form-field">
      <label>Теги</label>
      <span>
        <span v-for="(tag, index) in game.tags" :key="tag.id">
          <template v-if="index">, </template>
          <router-link :to="{ name: 'games', query: { tag: tag.id } }">{{ tag.title }}</router-link>
        </span>
      </span>
    </div>
    <div class="form-field" v-if="readers === null || readers.length">
      <label>Читатели</label>
      <span>
        <loader v-if="!readers" />
        <span v-else v-for="(reader, index) in readers" :key="reader.login">
          <template v-if="index">, </template>
          <user-link :user="reader" />
        </span>
      </span>
    </div>

    <characters-list />

    <div class="content-title">Об игре</div>
    <div v-html="game.info" />

  </div>

</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Game, GameStatus } from '@/api/models/gaming';
import CharactersList from './CharactersList.vue';
import { Route } from 'vue-router';
import { User } from '@/api/models/community';

const statuses: { [key: string]: { displayText: string; displayClass: string } } = {
  [GameStatus.Closed]: { displayText: 'Закрыта', displayClass: 'negative' },
  [GameStatus.Finished]: { displayText: 'Завершена', displayClass: 'negative' },
  [GameStatus.Frozen]: { displayText: 'Заморожена', displayClass: 'neutral' },
  [GameStatus.Requirement]: { displayText: 'Набор игроков', displayClass: 'positive' },
  [GameStatus.Draft]: { displayText: 'Оформляется', displayClass: 'neutral' },
  [GameStatus.Active]: { displayText: 'Идет игра', displayClass: 'positive' },
  [GameStatus.RequiresModeration]: { displayText: 'Требуется премодерация', displayClass: 'neutral' },
  [GameStatus.Moderation]: { displayText: 'На премодерации', displayClass: 'neutral' },
};

@Component({
  components: {
    CharactersList,
  }
})
export default class GameInformationPage extends Vue {
  private statuses = statuses;

  @Action('gaming/fetchSelectedGameReaders')
  private fetchReaders: any;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Getter('gaming/selectedGameReaders')
  private readers!: User[];

  @Watch('$route')
  private onRouteChanged(newValue: Route, oldValue: Route): void {
    if (newValue.params.id !== oldValue.params.id) {
      this.fetchData();
    }
  }

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    const id = this.$route.params.id;
    await this.fetchReaders({ id });
  }
}
</script>

<style scoped lang="stylus">
.page-loader
  square($big)
  margin $big auto

.game-disclaimer
  secondary()

.game-status-positive
  theme(color, $positiveText)

.game-status-negative
  theme(color, $negativeText)
</style>
