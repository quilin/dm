<template>
  <lightbox name="login">
    <template slot="title">Вход</template>
    <div class="form login-form">
      <label class="field-label">Логин</label>
      <div>
        <input type="text" v-model="data.login" />
      </div>
      <label class="field-label">Пароль</label>
      <div>
        <input type="password" v-model="data.password" />
      </div>
      <div class="login-remember">
        <label>
          <input type="checkbox" v-model="data.rememberMe" />
          Запомнить меня
        </label>
      </div>
    </div>
    <template slot="controls">
      <input type="button" @click="signIn" :disabled="formEmpty" value="Войти" />
      <a href="javascript:void(0)" @click="$modal.hide('login')">Отменить</a>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { Envelope, BadRequestError } from '@/api/models/common';
import { LoginCredentials, User } from '@/api/models/community';
import accountApi from '@/api/requests/accountApi';

@Component({})
export default class Login extends Vue {
  private data: LoginCredentials = {
    login: '',
    password: '',
    rememberMe: true,
  };

  private get formEmpty(): boolean {
    return !this.data.login && !this.data.password;
  }

  @Action('authenticate')
  private authenticate: any;

  private async signIn(): Promise<void> {
    const signResult = await accountApi.signIn(this.data);
    if ((signResult as Envelope<User>).resource) {
      const { resource } = signResult as Envelope<User>;
      this.authenticate(resource);
    } else {
      const requestError = signResult as BadRequestError;
      alert(requestError);
    }
  }
}
</script>

<style lang="stylus" scoped>
.login-form
  margin-top $medium

.login-remember
  grid-column 1 / span 2
</style>
