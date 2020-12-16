<template>
  <div>
    <div class="page-title">Новая игра</div>
    <form @submit.prevent="create">
      <div class="create-game__settings">

        <div class="create-game__general-settings">
          <div class="form-field">
            <label for="title">Название</label>
            <input id="title" v-model="game.title" />
          </div>
          <div class="form-field">
            <label for="system">Система</label>
            <input id="system" v-model="game.system" />
          </div>
          <div class="form-field">
            <label for="setting">Сеттинг</label>
            <input id="setting" v-model="game.setting" />
          </div>
          <div class="form-field">
            <label for="attribute-schema">Характеристики</label>
            <schema-selector id="attribute-schema" v-model="game.schema" />
          </div>
          <div class="form-field">
            <label for="commentaries-access">Обсуждения</label>
            <dropdown v-model="game.privacySettings.commentariesAccess" id="commentaries-access"
              :options="commentariesAccessOptions" />
          </div>
        </div>

        <div class="create-game__extended-settings">
          <a v-if="!extendedSettingsMode" @click="extendedSettingsMode = true">
            <icon :font="IconType.Settings" />
            Перейти к расширенным настройкам
          </a>
          <div v-else>

            Показывать всем
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewTemper" />
              характеры персонажей
            </label>
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewSkills" />
              навыки персонажей
            </label>
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewInventory" />
              инвентарь персонажей
            </label>
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewStory" />
              историю персонажей
            </label>
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewDice" />
              результаты бросков кубиков
            </label>
            <label class="form-field">
              <input type="checkbox" v-model="game.privacySettings.viewPrivates" />
              приватные сообщения
            </label>

          </div>
        </div>

      </div>

      <div class="form-field">
        <label for="info">Описание</label>
        <text-area id="info" class="create-game__info" v-model="game.info" />
      </div>
      <div class="form-field">
        <label for="tags">Теги</label>
        <tag-selector v-model="game.tags" id="tags" />
      </div>

      <action-button type="submit" :loading="creating">Создать игру</action-button>
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { CommentariesAccessMode, Game, Tag } from '@/api/models/gaming';
import { User } from '@/api/models/community';

import IconType from '@/components/iconType';
import CreateSchema from '@/views/pages/create-game/CreateSchema.vue';
import SchemaSelector from '@/views/pages/create-game/SchemaSelector.vue';
import TagSelector from '@/views/pages/create-game/TagSelector.vue';

@Component({
  components: {
    TagSelector,
    SchemaSelector,
    CreateSchema,
  },
})
export default class CreateGame extends Vue {
  private IconType: typeof IconType = IconType;
  private extendedSettingsMode = false;
  private creating = false;
  private game: Game = {
    privacySettings: {
      commentariesAccess: CommentariesAccessMode.Public,
    },
    schema: null,
    tags: [] as Tag[],
    assistant: {} as User,
  } as Game;
  private commentariesAccessOptions = [{
    value: CommentariesAccessMode.Public,
    label: 'доступны всем',
  }, {
    value: CommentariesAccessMode.Readonly,
    label: 'доступны посторонним только для чтения',
  }, {
    value: CommentariesAccessMode.Private,
    label: 'недоступны посторонним'
  }]

  @Getter('user')
  private user!: User | null;

  @Action('gaming/createGame')
  private createGame!: any;

  @Action('gaming/fetchOwnGames')
  private fetchOwnGames!: any;

  @Watch('user')
  private onUserChange(): void {
    this.authorize();
  }

  private async create(): Promise<void> {
    this.creating = true;
    await this.createGame({ game: this.game, $router: this.$router });
    this.creating = false;

    await this.fetchOwnGames();
  }

  private mounted(): void {
    this.authorize();
  }

  private authorize(): void {
    if (this.user === null) {
      this.$router.push({ name: 'home' })
    }
  }
}
</script>

<style lang="stylus" scoped>
.create-game__info
  flex-grow 1

.create-game__settings
  display flex

.create-game__general-settings
  width $gridStep * 100
  margin-right $medium

.create-game__extended-settings
  flex-grow 1
  padding-top $medium

  & .form-field
    margin $medium 0
    align-items end

    & input[type="checkbox"]
      margin-right $small
</style>
