<template>
  <div class="topic-comment">
    <template v-if="!editMode">
      <div v-html="comment.text" />
      <div class="topic-comment__meta">
        <div>
          <router-link :to="{name: 'user', params: {login: comment.author.login}}">
            <icon :font="IconType.User" />
            {{ comment.author.login }}
          </router-link>
          ,
          <human-timespan :date="comment.created" />
          <template v-if="comment.updated">
            (изменен
            <human-timespan :date="comment.updated" />
            )
          </template>
        </div>
        <div class="topic-comment__controls" v-if="commentOwner">
          <a class="topic-comment__control" @click="editComment">
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
    <edit-comment-form v-else :comment="comment" @edited="onEdit" />
    <delete-comment-lightbox :comment-id="comment.id" @deleted="onDelete" />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import { Comment } from '@/api/models/forum';
import { userIsAdmin } from '@/api/models/community/helpers';
import IconType from '@/components/iconType';
import EditCommentForm from '@/views/pages/topic/EditCommentForm.vue';
import DeleteCommentLightbox from '@/views/pages/topic/DeleteCommentLightbox.vue';

@Component({
  components: { DeleteCommentLightbox, EditCommentForm },
})
export default class TopicComment extends Vue {
  private IconType: typeof IconType = IconType;

  private editMode = false;

  @Prop()
  private comment!: Comment;

  @Getter('user')
  private user!: User | null;

  private get commentOwner() {
    return this.comment.author.login === this.user?.login || userIsAdmin(this.user);
  }

  private editComment() {
    this.editMode = true;
  }

  private onEdit() {
    this.editMode = false;
  }

  private onDelete() {
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
