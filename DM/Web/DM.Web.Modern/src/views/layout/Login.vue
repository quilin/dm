<template>
  <confirm-lightbox
      name="login"
      title="Вход"
      accept-text="Войти"
      @accepted="signIn"
      @canceled="$modal.hide('login')"
  >
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
  </confirm-lightbox>
</template>

<script lang="ts">
import { Vue } from 'vue-property-decorator';
import Component from 'vue-class-component';
import { Action } from 'vuex-class';
import ConfirmLightbox from '@/components/ConfirmLightbox.vue';

@Component({
  components: { ConfirmLightbox },
})
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
