<template>
  <lightbox name="login">
    <template slot="title">Вход</template>
    <div class="form">
      <div class="form-field" :class="{ error: $v.login.$error }">
        <label class="form-field-label">Логин</label>
        <input type="text" v-model.trim="$v.login.$model" />
        <div v-if="$v.login.$error" class="form-field-error"></div>
      </div>
      <div class="form-field" :class="{ error: $v.password.$error }">
        <label class="form-field-label">Пароль</label>
        <input type="password" v-model="$v.password.$model" />
        <div v-if="$v.password.$error" class="form-field-error">$v.password.$error</div>
      </div>
      <div class="form-field">
        <label>
          <input type="checkbox" v-model="rememberMe" />
          Запомнить меня
        </label>
      </div>
    </div>
    <template slot="controls">
      <input type="button" @click="signIn" :disabled="$v.$error" value="Войти" />
      <a href="javascript:void(0)" @click="$modal.hide('login')">Отменить</a>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Vue } from 'vue-property-decorator';
import Component from 'vue-class-component';
import { required } from 'vuelidate/lib/validators';
import { Action } from 'vuex-class';
import accountApi from '@/api/requests/accountApi';

@Component({
  validations: {
    login: {
      required,
    },
    password: {
      required,
    },
  },
})
export default class Login extends Vue {
  private login: string = '';
  private password: string = '';
  private rememberMe: boolean = false;

  private validations: any = {
    login: {
      required,
    },
    passwod: {
      required,
    },
  };

  @Action('authenticate')
  private authenticate: any;

  private async signIn(): Promise<void> {
    const { login, password, rememberMe } = this;
    const { data, error } = await accountApi.signIn({ login, password, rememberMe });
    if (error) {
      alert(JSON.stringify(error));
    } else {
      this.authenticate(data!.resource);
    }
  }
}
</script>

<style lang="stylus">
</style>
