<template>
  <form ref="form" @submit.prevent="createComment">
    <div class="content-title">Добавить комментарий</div>
    <text-area :disabled="loading" v-model.trim="text" />
    <action-button class="comment-form__submit" type="submit" :loading="loading" :disabled="!text">
      Добавить
    </action-button>
  </form>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

@Component({})
export default class CreateCommentForm extends Vue {
  private text = '';

  private loading = false;

  @Action('forum/createComment')
  private createCommentAction: any;

  @Getter('forum/selectedTopic')
  private selectedTopic!: string;

  private async createComment() {
    this.loading = true;

    await this.createCommentAction({
      id: this.selectedTopic,
      comment: {
        text: this.text,
      },
      router: this.$router,
    });

    this.loading = false;

    this.text = '';
  }
}
</script>

<style scoped lang="stylus">
.comment-form
  &__submit {
    margin-top $minor;
  }
</style>
