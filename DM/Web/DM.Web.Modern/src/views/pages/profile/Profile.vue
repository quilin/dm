<template>
  <div>

    <loader v-if="!user" :big="true" />

    <div v-else class="container">

      <div class="title">
        {{user.login}}
        <span class="roles">{{userRoles.join(', ')}}</span>
      </div>

      <div class="info">
        <div class="stats">

          <div class="picture-container">
            <div class="picture" :style="{ backgroundImage: user.pictureUrl ? `url(${user.pictureUrl})` : undefined }" />
            <div class="picture-upload" v-if="canUploadPicture">
              <icon :font="IconType.Upload" /> Загрузить изображение
              <upload @uploading="uploadProfilePicture" />
            </div>
          </div>

          <profile-stat title="В сети"><online :user="user" :detailed="true" /></profile-stat>
          <profile-stat title="Рейтинг"><rating :user="user" /></profile-stat>
          <profile-stat title="На сайте"><human-date :date="user.registration" format="DD.MM.YYYY" /></profile-stat>
          <profile-stat title="Как зовут">{{user.name || '—'}}</profile-stat>
          <profile-stat title="Где живет">{{user.location || '—'}}</profile-stat>
          <profile-stat title="Skype">{{user.skype || '—'}}</profile-stat>
        </div>
        <div class="details">

          <div class="tab-links">
            <router-link :to="{ name: 'profile' }">О себе</router-link>
            <router-link :to="{ name: 'profile-games' }">Игры</router-link>
            <router-link :to="{ name: 'profile-characters' }">Персонажи</router-link>
            <router-link v-if="isOwnUser" :to="{ name: 'profile-settings' }">Настройки</router-link>
          </div>

          <router-view />

        </div>
      </div>

    </div>

  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Route } from 'vue-router';

import { User, UserRole } from '@/api/models/community';

import ProfileStat from './ProfileStat.vue';
import IconType from '@/components/iconType';
import communityApi from '@/api/requests/communityApi';

const roleNames: Record<string, string> = {
  [UserRole.Administrator]: 'Тролль',
  [UserRole.SeniorModerator]: 'Старший гоблин',
  [UserRole.RegularModerator]: 'Гоблин',
  [UserRole.NannyModerator]: 'Гоблин-нянька',
};

@Component({
  components: {
    ProfileStat,
  },
})
export default class Profile extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('user')
  private currentUser!: User;

  @Getter('community/selectedUser')
  private user!: User | null;

  @Action('community/selectUser')
  private selectUser: any;

  @Watch('$route')
  private onRouteChanged(newValue: Route, oldValue: Route): void {
    if (newValue.params.login !== oldValue.params.login) {
      this.fetchData();
    }
  }

  private get isOwnUser(): boolean {
    return this.user?.login === this.currentUser?.login;
  }

  private get canUploadPicture(): boolean {
    return this.isOwnUser || this.currentUser?.roles.some(role => role === UserRole.Administrator);
  }

  private get userRoles(): string[] {
    return this.user!.roles
      .filter((r: UserRole) => r in roleNames)
      .map((r: UserRole) => roleNames[r]);
  }

  private uploadProfilePicture(formData: FormData): void {
    communityApi.uploadUserPicture(this.user!.login, formData, this.onUploadProgress);
  }

  private onUploadProgress(data: any) {
    console.log(data);
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    this.selectUser({ login: this.$route.params.login, router: this.$router });
  }
}
</script>

<style scoped lang="stylus">
.title
  pageTitle()

.roles
  secondary()

.info
  display flex
  margin-top $gridStep * 5

.stats
  width $gridStep * 50
  margin-right $medium
  flex-shrink 0

.picture-container
  position relative
  margin-bottom $medium

.picture
  margin 0 auto
  width $large
  height $large
  border-radius $large
  themeExtend(box-shadow, inset 0 0 $minor, $border)
  background 0 0 no-repeat
  background-size cover
  background-image url('~@/assets/userpic.png')

.picture-upload
  position absolute
  bottom 0
  left 0
  right 0
  opacity 1

  .picture:hover &
    font-weight bold

.details
  flex-grow 1

.tab-links
  tab-links()
</style>
