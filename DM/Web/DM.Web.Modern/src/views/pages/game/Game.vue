<template>
  <div>
    <router-view v-if="game" />
    <loader v-else :big="true" />
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Game } from '@/api/models/gaming';
import { Route } from 'vue-router';

@Component({})
export default class GamePage extends Vue {
  @Action('gaming/selectGame')
  private selectGame: any;

  @Getter('gaming/selectedGame')
  private game!: Game;

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
    await this.selectGame({ id, router: this.$router });
  }
}
</script>

<style scoped lang="stylus">
.page-loader
  square($big)
  margin $big auto
</style>
