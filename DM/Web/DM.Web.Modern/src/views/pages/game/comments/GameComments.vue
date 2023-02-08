<template>
  <div>
    <div class="page-title">{{ game.title }} | Обсуждение</div>
    <router-view />
    <create-game-comment-form v-if="canCreateComments" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { CommentariesAccessMode, Game, GameParticipation } from '@/api/models/gaming';
import CreateGameCommentForm from '@/views/pages/game/comments/CreateGameCommentForm.vue';

@Component({
  components: { CreateGameCommentForm }
})
export default class GameComments extends Vue {
  @Getter('gaming/selectedGame')
  private game!: Game;

  @Action('gaming/fetchSelectedGameCharacters')
  private fetchGameCharacters: any;

  private get canCreateComments() {
    return this.game.privacySettings.commentariesAccess === CommentariesAccessMode.Public ||
        this.game.participation.some(p => p !== GameParticipation.None && p !== GameParticipation.Reader);
  }

  private mounted() {
    this.fetchGameCharacters({ id: this.$route.params.id });
  }
}
</script>

<style lang="stylus"></style>
