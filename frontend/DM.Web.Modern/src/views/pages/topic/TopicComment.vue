<template>
  <div class="container">
    <div class="wrapper">
      <router-link class="user-picture-container" :to="{ name: 'profile', params: { login: comment.author.login } }">
        <div class="user-picture" :style="{ backgroundImage: comment.author.mediumPictureUrl ? `url(${comment.author.mediumPictureUrl})` : null }" />
      </router-link>
      <div class="content">
        <div class="meta">
          <user-link :user="comment.author" :hide-picture="true" />,
          <human-timespan :date="comment.created" />
          <template v-if="comment.updated">
            , (изменен
            <human-timespan :date="comment.updated" />)
          </template>
          &nbsp;
          <like :entity="comment" @liked="addLike({ id: comment.id })" @unliked="deleteLike({ id: comment.id })" />
        </div>
        <div v-if="!editMode" v-html="comment.text" />
        <edit-comment-form v-else :comment="comment" @edited="hideEditForm" @canceled="hideEditForm" />
        <div v-if="actionsAvailable" class="actions" @click="editMode = true">
          <icon :font="IconType.Kebab" />
        </div>
      </div>
    </div>
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
    return this.comment.author!.login === this.user?.login ||
        this.moderators.some(moderator => moderator.login === this.user?.login) ||
        userIsHighAuthority(this.user);
  }

  private get actionsAvailable(): boolean {
    return !this.editMode && this.canEdit;
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
.container
  margin $small (-($gridStep * 3))
  padding $gridStep * 3
  border-radius $borderRadius

.wrapper
  display flex

.user-picture-container
  display block
  height $gridStep * 10
  flex-shrink 0

.user-picture
  width $gridStep * 10
  height $gridStep * 10
  margin-right $gridStep * 3
  background transparent left center no-repeat
  background-size contain
  border-radius $big

.content
  position relative
  flex-grow 1

.meta
  theme(color, $secondaryText)
  margin-bottom $minor

.actions
  position absolute
  right 0
  top 0
</style>
