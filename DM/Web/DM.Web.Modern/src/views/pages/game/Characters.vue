<template>
  <div>
    <div class="page-title">{{ game.title }} | Персонажи</div>

    <loader v-if="!characters" :big="true" />

    <div v-else-if="!characters.length" class="characters-empty">
      Пока не подано ни одной заявки
    </div>

    <character-details v-else v-for="character in characters" :key="character.id"
      :character="character"
      :privacy-settings="game.privacySettings" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Character, Game } from '@/api/models/gaming';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import CharacterDetails from '@/views/pages/game/CharacterDetails.vue';


@Component({
  components: { CharacterDetails }
})
export default class CharactersPage extends Vue {
  @Action('gaming/fetchSelectedGameCharacters')
  private fetchCharacters: any;

  @Getter('user')
  private user!: User | null;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Getter('gaming/selectedGameCharacters')
  private characters!: Character[];

  private mounted() {
    this.fetchData();
  }

  private fetchData() {
    this.fetchCharacters({ id: this.game.id });
  }
}
</script>

<style lang="stylus">
</style>