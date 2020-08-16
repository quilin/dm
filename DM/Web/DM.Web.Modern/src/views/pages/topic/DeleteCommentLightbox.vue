<template>
  <lightbox name="delete-comment">
    <template slot="title">Удалить комментарий?</template>
    <template slot="controls">
      <button @click="deleteComment" class="delete-comment-lightbox__submit">Удалить</button>
      <a @click="$modal.hide('delete-comment')">Отменить</a>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Action } from 'vuex-class';

@Component({})
export default class DeleteCommentLightbox extends Vue {
  @Prop()
  private commentId!: string;

  @Action('forum/deleteComment')
  private deleteCommentAction: any;

  private async deleteComment() {
    this.deleteCommentAction({ id: this.commentId });
    this.$emit('deleted');
  }
}
</script>

<style scoped lang="stylus">
.delete-comment-lightbox
  &__submit
    margin-right $medium
</style>
