<template>
  <div class="header">
    <div class="header-block">
      <div class="title">
        <slot name="title"/>
      </div>
      <div class="subtitle">
        <slot name="subtitle"/>
      </div>
    </div>
    <div class="user">
      <a href="javascript:void(0)" v-if="user" @click="signOut"><icon :font="IconType.Logout" /></a>
      <router-link v-else :to="{name: 'UserLogin'}" class="button">Войти</router-link>
    </div>
  </div>

</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import {Action, Getter} from 'vuex-class';
  import {User} from '@/api/models/community';
  import IconType from '@/components/iconType';

  @Component
  export default class Header extends Vue {
    private IconType: typeof IconType = IconType;

    @Getter('user')
    private user!: User | null;

    @Action('signOut')
    private signOut: any;
  }
</script>

<style scoped lang="stylus">
  .header
    padding 0 $small
    display flex
    flex-direction row
    align-items center
    justify-content space-between
    height 100%

    .header-block
      display flex
      flex-direction column
      align-items left
      justify-content center
      height 100%

      .title
        font-size $titleFontSize
        theme(color, $text)

      .subtitle
        font-size $secondaryFontSize
        theme(color, $secondaryText)

    .user
      .icon
        iconBig()
</style>
