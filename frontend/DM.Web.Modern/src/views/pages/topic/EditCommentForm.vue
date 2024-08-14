<template>
  <form @submit.prevent="saveComment">
    <text-area v-model="text" />
    <action-button class="submit" type="submit" :loading="loading" :disabled="!text">
      Сохранить
    </action-button>

    <a @click="cancel" class="cancel">Отменить</a>
  </form>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Action } from 'vuex-class';
import { Comment } from '@/api/models/forum';
import forumApi from '@/api/requests/forumApi';

@Component({})
export default class EditCommentForm extends Vue {
  private text = '';

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

    this.text = commentForUpdate.text;

    this.loading = false;
  }

  private async saveComment() {
    this.loading = true;

    await this.updateComment({
      id: this.comment.id,
      comment: {
        text: this.text,
      },
    })

    this.loading = false;

    this.$emit('edited');
  }

  private cancel() {
    this.$emit('canceled');
  }
}
</script>

<style scoped lang="stylus">
.submit
    margin-top $minor
    margin-right $medium

.cancel
    font-size 14px
</style>
