<template>
  <div>

    <template v-if="game">
      <div class="content-title">{{game.title}}</div>

      <div class="game-actions">
        <div>
          <template v-if="rooms === null">
            Ходы игроков <loader style="display: inline-block" />
          </template>
          <router-link v-else-if="rooms.length === 1 && rooms[0].title === 'Основная комната'"
            :to="{ name: 'game-room', params: { id: rooms[0].id } }">Ходы игроков</router-link>
          <template v-else>
            Ходы игроков
            <div class="game-actions">
              <div v-for="room in rooms" :key="room.id">
                <router-link :to="{ name: 'game-room', params: { id: room.id } }">{{room.title}}</router-link>
              </div>
            </div>
          </template>
        </div>
        <div v-if="canReadComments">
          <router-link :to="{ name: 'game-comments', params: { id: game.id } }">
            Обсуждение
            <template v-if="game.unreadCommentsCount">
              <icon :font="IconType.CommentsUnread" /> {{game.unreadCommentsCount}}
            </template>
          </router-link>
        </div>
        <div>
          <router-link :to="{ name: 'game', params: { id: $route.params.id } }">Информация</router-link>
        </div>
        <div>
          <router-link :to="{ name: 'game-characters', params: { id: $route.params.id } }">
            Персонажи
            <template v-if="game.unreadCharactersCount">
              <icon :font="IconType.User" /> {{game.unreadCharactersCount}}
            </template>
          </router-link>
        </div>
      </div>

      <div v-if="user" class="game-user-actions">
        <template v-if="masters">
          <div><router-link :to="{ name: 'game-settings', params: { id: game.id } }">
            Настройки
          </router-link></div>
          <div><router-link :to="{ name: 'create-character', params: { id: game.id } }">
            Новый персонаж
          </router-link></div>
        </template>
        <div v-else-if="plays" class="game-player-actions">

        </div>
        <div v-else class="game-non-participant-actions">
          <div>
            <router-link :to="{ name: 'create-character', params: { id: game.id } }" class="game-apply">
              Подать заявку на участие
            </router-link>
          </div>
          <div>
            <a v-if="subscribed" @click="stopObservation">Прекратить наблюдение</a>
            <a v-else @click="startObservation">Наблюдать за игрой</a>
            <loader v-if="subscriptionLoading" class="game-subscription-loader" />
          </div>
        </div>
      </div>
    </template>

    <own-games v-if="user" />
    <fora />

  </div>
</template>

<script lang="ts">
import {Component, Vue, Watch} from 'vue-property-decorator';
import {Action, Getter} from 'vuex-class';

import {User} from '@/api/models/community';
import {CommentariesAccessMode, Game, GameParticipation, Room} from '@/api/models/gaming';
import IconType from '@/components/iconType';

import OwnGames from '../../layout/menu/OwnGames.vue';
import Fora from '../../layout/menu/Fora.vue';
import {Route} from 'vue-router';

@Component({
  components: {
    OwnGames,
    Fora,
  },
})
export default class GameMenu extends Vue {
  private IconType: typeof IconType = IconType;
  private subscriptionLoading = false;

  @Action('gaming/fetchSelectedGameRooms')
  private fetchRooms: any;

  @Action('gaming/subscribe')
  private subscribe: any;

  @Action('gaming/unsubscribe')
  private unsubscribe: any;

  @Getter('user')
  private user!: User;

  @Getter('gaming/selectedGame')
  private game!: Game | null;

  @Getter('gaming/selectedGameRooms')
  private rooms!: Room[];

  private get canReadComments(): boolean {
    return this.game !== null &&
        (this.game.privacySettings.commentariesAccess !== CommentariesAccessMode.Private ||
        this.game.participation.some(p => p !== GameParticipation.None && p !== GameParticipation.Reader));
  }

  private get masters(): boolean {
    return this.game!.participation.some(p =>
      p === GameParticipation.Authority ||
      p === GameParticipation.Owner);
  }

  private get plays(): boolean {
    return this.game!.participation.some(p => p === GameParticipation.Player);
  }

  private get subscribed(): boolean {
    return this.game!.participation.some(p => p === GameParticipation.Reader);
  }

  private async startObservation() {
    if (this.subscriptionLoading) return;
    this.subscriptionLoading = true;
    await this.subscribe({ router: this.$router });
    this.subscriptionLoading = false;
  }

  private async stopObservation() {
    if (this.subscriptionLoading) return;
    this.subscriptionLoading = true;
    await this.unsubscribe({ router: this.$router });
    this.subscriptionLoading = false;
  }

  @Watch('$route')
  private onRouteChanged(oldRoute: Route, newRoute: Route): void {
    if (oldRoute.matched.some(r => r.parent?.name === 'game') &&
      newRoute.matched.some(r => r.parent?.name === 'game') &&
      oldRoute.params.id !== newRoute.params.id) {
      this.fetchData();
    }
  }

  private mounted(): void {
    this.fetchData();
  }

  private async fetchData() {
    await this.fetchRooms({ id: this.$route.params.id });
  }
}
</script>

<style scoped lang="stylus">
.game-user-actions
  margin-top $medium

.game-apply
  font-weight bold

.game-subscription-loader
  display inline-block
  margin-left $minor
</style>
