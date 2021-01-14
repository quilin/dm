<template>
  <div>

    <div class="page-title">{{ game.title }} | Обсуждение</div>

    <router-view />

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Game } from '@/api/models/gaming';

@Component({})
export default class GameComments extends Vue {
  @Getter('gaming/selectedGame')
  private game!: Game;

  @Action('gaming/fetchSelectedGameCharacters')
  private fetchGameCharacters: any;

  private mounted() {
    this.fetchGameCharacters({ id: this.$route.params.id });
  }
}
</script>

<style lang="stylus"></style>