<template>
  <div class="game-comment">
    <template v-if="!editMode">
      <div v-html="comment.text" />
      <div class="game-comment__meta">
        <div>
          <user-link :user="comment.author" />

          (участие?!)

          <human-timespan :date="comment.created" />
          <template v-if="comment.updated">
            , (изменен <human-timespan :date="comment.updated" />)
          </template>

          <like :entity="comment" @liked="addLike" @unliked="deleteLike" />
        </div>
        <div class="game-comment__controls" v-if="canEdit">
          <a class="game-comment__control" @click="editMode = true">
            <icon :font="IconType.Edit" />
            Редактировать
          </a>
          <a class="game-comment__control" @click="$modal.show('delete-game-comment')">
            <icon :font="IconType.Remove" />
            Удалить
          </a>
        </div>
      </div>
    </template>
    <edit-game-comment-form v-else :comment="comment" @edited="editMode = false" @canceled="editMode = false" />
    <confirm-lightbox
        name="delete-game-comment"
        title="Удалить комментарий?"
        accept-text="Удалить"
        @accepted="deleteComment"
        @canceled="$modal.hide('delete-game-comment')"
    />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Character, Comment, Game, GameParticipation } from '@/api/models/gaming';
import Like from '@/components/shared/Like.vue';
import ConfirmLightbox from '@/components/ConfirmLightbox.vue';
import { Getter } from 'vuex-class';
import { userIsHighAuthority } from '@/api/models/community/helpers';
import { User } from '@/api/models/community';
import IconType from '@/components/iconType';

@Component({
  components: { ConfirmLightbox, Like }
})
export default class GameComment extends Vue {
  private IconType: typeof IconType = IconType;
  private editMode = false;

  @Prop()
  private comment!: Comment;

  @Getter('user')
  private user!: User | null;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Getter('gaming/selectedGameCharacters')
  private characters!: Character[];

  private get canEdit() {
    return this.game.participation.some(p => p === GameParticipation.Authority) ||
        userIsHighAuthority(this.user) ||
        this.comment.author.login === this.user?.login;
  }

  private addLike(): void {
    console.log('liked');
  }
  private deleteLike(): void {
    console.log('unliked');
  }
  private deleteComment(): void {
    console.log('deleted');
    this.$modal.hide('delete-game-comment');
  }
}
</script>
