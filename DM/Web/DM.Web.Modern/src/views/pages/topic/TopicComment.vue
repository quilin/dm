<template>
  <div class="topic-comment">
    <template v-if="!editMode">
      <div v-html="comment.text" />
      <div class="topic-comment__meta">
        <div>
          <user-link :user="comment.author" />,
          <human-timespan :date="comment.created" />
          <template v-if="comment.updated">
            , (изменен
            <human-timespan :date="comment.updated" />)
          </template>
          &nbsp;
          <like :entity="comment" @liked="addLike({ id: comment.id })" @unliked="deleteLike({ id: comment.id })" />
        </div>
        <div class="topic-comment__controls" v-if="canEdit">
          <a class="topic-comment__control" @click="showEditForm">
            <icon :font="IconType.Edit" />
            Редактировать
          </a>
          <a class="topic-comment__control" @click="$modal.show('delete-comment')">
            <icon :font="IconType.Remove" />
            Удалить
          </a>
        </div>
      </div>
    </template>
    <edit-comment-form v-else :comment="comment" @edited="hideEditForm" @canceled="hideEditForm" />
    <confirm-lightbox
        name="delete-comment"
        title="Удалить комментарий?"
        accept-text="Удалить"
        @accepted="deleteComment"
        @canceled="$modal.hide('delete-comment')"
    />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Comment } from '@/api/models/forum';
import IconType from '@/components/iconType';
import EditCommentForm from '@/views/pages/topic/EditCommentForm.vue';
import ConfirmLightbox from '@/components/ConfirmLightbox.vue';
import Like from '@/components/shared/Like.vue';
import { Action, Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import { userIsHighAuthority } from '@/api/models/community/helpers';

@Component({
  components: { ConfirmLightbox, EditCommentForm, Like },
})
export default class TopicComment extends Vue {
  private IconType: typeof IconType = IconType;

  private editMode = false;

  @Prop()
  private comment!: Comment;

  @Action('forum/deleteComment')
  private deleteCommentAction: any;

  @Action('forum/deleteCommentLike')
  private deleteLike: any;

  @Action('forum/addCommentLike')
  private addLike: any;

  @Getter('forum/moderators')
  private moderators!: User[];

  @Getter('user')
  private user!: User | null;

  private get canEdit() {
    return this.comment.author.login === this.user?.login ||
        this.moderators.some(moderator => moderator.login === this.user?.login) ||
        userIsHighAuthority(this.user);
  }

  private showEditForm() {
    this.editMode = true;
  }

  private hideEditForm() {
    this.editMode = false;
  }

  private deleteComment() {
    this.deleteCommentAction({ id: this.comment.id });

    this.$emit('deleted');
  }
}
</script>

<style scoped lang="stylus">
.topic-comment
  panel()

  padding $minor
  border 1px solid
  theme(border-color, $border)

  &__meta
    display flex
    justify-content space-between

    margin-top $minor

    secondary()

  &__controls
    display none

  &:hover &__controls
    display block

  &__control
    margin-left $medium
</style>
