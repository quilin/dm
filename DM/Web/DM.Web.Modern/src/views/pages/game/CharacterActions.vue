<template>
  <div class="container">
    <template v-if="userIsGameAuthority">

      <template v-if="character.status === CharacterStatus.Registration">
        <div>
          <a @click="setStatus(CharacterStatus.Active)">
            <icon :font="IconType.Tick" />
            Принять
          </a>
        </div>
        <div>
          <a @click="setStatus(CharacterStatus.Declined)">
            <icon :font="IconType.Close" />
            Отклонить
          </a>
        </div>
      </template>
      <template v-else-if="character.status === CharacterStatus.Active">
        <div>
          <a @click="setStatus(CharacterStatus.Declined)">
            <icon :font="IconType.Close" />
            Отклонить
          </a>
        </div>
        <div>
          <a @click="setStatus(CharacterStatus.Dead)">
            <icon :font="IconType.ArrowDown" />
            Убить
          </a>
        </div>
      </template>
      <template v-else-if="character.status === CharacterStatus.Dead">
        <div>
          <a @click="setStatus(CharacterStatus.Active)">
            <icon :font="IconType.ArrowUp" />
            Воскресить
          </a>
        </div>
      </template>

    </template>

    <template v-else-if="userIsCharacterOwner()">

    </template>

  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Character, CharacterStatus, Game, GameParticipation } from '@/api/models/gaming';
import { Getter } from 'vuex-class';
import { User } from '@/api/models/community';
import IconType from '@/components/iconType';

@Component({})
export default class CharacterActions extends Vue {
  private IconType: typeof IconType = IconType;
  private CharacterStatus: typeof CharacterStatus = CharacterStatus;

  @Prop()
  private character!: Character;

  @Getter('user')
  private user!: User | null;

  @Getter('gaming/selectedGame')
  private game!: Game;

  private get userIsGameAuthority(): boolean {
    return this.game.participation.some(p => p === GameParticipation.Authority);
  }

  private userIsCharacterOwner(): boolean {
    return this.user?.login === this.character.author.login;
  }

  private async setStatus(status: CharacterStatus): Promise<void> {
    console.log(this.character, status);
  }
}
</script>

<style lang="stylus">
.container
  flex-shrink 0
</style>