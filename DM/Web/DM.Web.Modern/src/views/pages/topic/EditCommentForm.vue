<template>
  <form class="edit-comment-form" @submit.prevent="saveComment">
    <text-area v-model="bbCommentText" :disabled="loading" />
    <button class="edit-comment-form__save" type="submit" :disabled="loading">Сохранить</button>
  </form>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Action } from 'vuex-class';
import { Comment } from '@/api/models/forum';
import forumApi from '@/api/requests/forumApi';

@Component({})
export default class EditCommentForm extends Vue {
  private bbCommentText = '';

  private loading = false;

  @Prop()
  private comment!: Comment;

  @Action('forum/updateComment')
  private updateComment: any;

  private mounted() {
    this.fetchData();
  }

  private async fetchData() {
    this.loading = true;

    const commentForUpdate = await forumApi.getCommentForUpdate(this.comment.id);

    this.bbCommentText = commentForUpdate.text;

    this.loading = false;
  }

  private async saveComment() {
    this.loading = true;

    await this.updateComment({
      id: this.comment.id,
      comment: {
        text: this.bbCommentText,
      },
    })

    this.loading = false;

    this.$emit('edited');
  }
}
</script>

<style scoped lang="stylus">
.edit-comment-form
  &__save
    margin-top $minor
</style>
