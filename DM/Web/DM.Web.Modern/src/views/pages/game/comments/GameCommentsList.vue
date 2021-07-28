<template>
  <div>

    <loader v-if="!comments" :big="true" />

    <template v-else-if="comments.resources.length">
      <paging :paging="comments.paging" :to="{ name: 'game-comments', params: $route.params }" />
      <game-comment v-for="comment in comments.resources" :key="comment.id" :comment="comment" />
    </template>

    <div v-else>Пока никто ничего не написал</div>

  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Character, Game, Comment } from '@/api/models/gaming';
import { ListEnvelope } from '@/api/models/common';
import GameComment from '@/views/pages/game/comments/GameComment.vue';

@Component({
  components: {GameComment}
})
export default class GameComments extends Vue {
  @Getter('gaming/selectedGameComments')
  private comments!: ListEnvelope<Comment> | null;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Getter('gaming/selectedGameCharacters')
  private characters!: Character[] | null;

  @Action('gaming/fetchSelectedGameComments')
  private fetchGameComments: any;

  @Watch('$route')
  private onRouteChange() {
    this.fetchData();
  }

  private mounted() {
    this.fetchData();
  }

  private fetchData() {
    this.fetchGameComments({ id: this.game.id, n: this.$route.params.n });
  }
}
</script>

<style lang="stylus"></style>
