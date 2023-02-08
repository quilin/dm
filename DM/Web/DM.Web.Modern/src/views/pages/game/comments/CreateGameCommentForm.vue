<template>
  <form ref="form" @submit.prevent="createComment">
    <div class="content-title">Добавить комментарий</div>
    <text-area :disabled="loading" v-model.trim="comment.text" />
    <action-button class="comment-form__submit" type="submit" :loading="loading" :disabled="!comment.text">
      Добавить
    </action-button>
  </form>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Comment, Game } from '@/api/models/gaming';

@Component({})
export default class CreateGameCommentForm extends Vue {
  private comment = { text: '' } as Comment;
  private loading = false;

  @Action('gaming/createComment')
  private createCommentAction: any;

  @Getter('gaming/selectedGame')
  private selectedGame!: Game;

  private async createComment() {
    this.loading = true;
    await this.createCommentAction({
      comment: this.comment,
      $router: this.$router,
    });
    this.loading = false;

    this.comment = { text: '' } as Comment;
  }
}
</script>

<style scoped lang="stylus">
.comment-form
  &__submit {
    margin-top $minor;
  }
</style>
