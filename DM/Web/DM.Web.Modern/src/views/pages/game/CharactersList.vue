<template>
  <div>

    <div class="content-title">Персонажи</div>

    <loader v-if="!characters" />

    <div v-else-if="!characters.length" class="characters-empty">
      Пока не подано ни одной заявки
    </div>

    <template v-else>
      <div class="row-title">
        <div>#</div>
        <div>Имя / Раса / Класс</div>
        <div>Автор</div>
        <div>Рейтинг</div>
        <div>В сети</div>
        <div>Ходы</div>
        <div>Статус</div>
      </div>
      <div v-for="(character, index) in characters" :key="character.id" class="row">
        <div class="row-counter">{{ index + 1 }}</div>
        <router-link :to="{name: 'game-characters', params: { id: game.id, characterId: character.id }}">
          {{ character.name }}
          <div class="character-details">{{ [character.race, character.class].filter(s => s).join(' / ') }}</div>
        </router-link>
        <div>
          <user-link :user="character.author" />
        </div>
        <div>
          <rating :user="character.author" />
        </div>
        <div>
          <online :user="character.author" />
        </div>
        <div>{{ character.totalPostsCount }}</div>
        <div :class="['character-status', `character-status-${statuses[character.status].displayClass}`]">
          {{ statuses[character.status].displayText }}
        </div>
      </div>
    </template>

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import IconType from '@/components/iconType';
import { Game, Character, CharacterStatus } from '@/api/models/gaming';

const characterStatuses: { [key: string]: { displayText: string; displayClass: string } } = {
  [CharacterStatus.Registration]: { displayText: 'заявка', displayClass: 'neutral' },
  [CharacterStatus.Active]: { displayText: 'в игре', displayClass: 'positive' },
  [CharacterStatus.Declined]: { displayText: 'отклонен', displayClass: 'negative' },
  [CharacterStatus.Left]: { displayText: 'вышел', displayClass: 'neutral' },
  [CharacterStatus.Dead]: { displayText: 'мертв', displayClass: 'negative' },
};

@Component({})
export default class CharactersList extends Vue {
  private IconType: typeof IconType = IconType;
  private statuses = characterStatuses;

  @Action('gaming/fetchSelectedGameCharacters')
  private fetchCharacters: any;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Getter('gaming/selectedGameCharacters')
  private characters!: Character[];

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    this.fetchCharacters({ id: this.game.id });
  }
}
</script>

<style scoped lang="stylus">
@import 'Grid.styl'

.characters-empty
  secondary()

.row-title
  gridHead($charactersGridTemplate)

.row
  grid($charactersGridTemplate)

.row-divider
  gridHead(\[all\] 100%)

.character-status-positive
  theme(color, $positiveText)

.character-status-negative
  theme(color, $negativeText)

.character-details
  secondary()
</style>
