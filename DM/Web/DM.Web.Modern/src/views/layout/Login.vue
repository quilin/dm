<template>
  <lightbox name="login">
    <template slot="title">Вход</template>
    <div class="form">
      <div class="form-field">
        <label class="form-field-label">Логин</label>
        <input type="text" v-model.trim="login" />
      </div>
      <div class="form-field">
        <label class="form-field-label">Пароль</label>
        <input type="password" v-model="password" />
      </div>
      <label>
        <input type="checkbox" v-model="rememberMe" />
        Запомнить меня
      </label>
    </div>
    <template slot="controls">
      <action-button @click="signIn">Войти</action-button>
      <a href="javascript:void(0)" @click="$modal.hide('login')">Отменить</a>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Vue } from 'vue-property-decorator';
import Component from 'vue-class-component';
import { Action } from 'vuex-class';

@Component({})
export default class Login extends Vue {
  private login = '';
  private password = '';
  private rememberMe = false;

  @Action('authenticate')
  private authenticate: any;

  private async signIn(): Promise<void> {
    const { login, password, rememberMe } = this;
    this.authenticate({ login, password, rememberMe });
  }
}
</script>

<style lang="stylus" scoped>
</style>
