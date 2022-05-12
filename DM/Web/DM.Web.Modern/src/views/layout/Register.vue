<template>
  <div>
    <confirm-lightbox
      name="register"
      title="Регистрация"
      accept-text="Зарегистрироваться"
      @accepted="register"
      @canceled="$modal.hide('register')">
      <div class="form">
        <div class="form-field">
          <label class="form-field-label">E-mail</label>
          <input type="text" v-model.trim="email" />
        </div>
        <div class="form-field">
          <label class="form-field-label">Логин</label>
          <input type="text" v-model.trim="login" />
        </div>
        <div class="form-field">
          <label class="form-field-label">Пароль</label>
          <input type="password" v-model="password" />
        </div>
      </div>
    </confirm-lightbox>

    <lightbox :name="successLightboxName">
      <template v-slot:title>Регистрация прошла успешно!</template>
      <div>
        Учетная запись <strong>{{login}}</strong> создана и ожидает подтверждения.<br />
        Вам на почту <strong>{{email}}</strong> отправлено письмо, содержащее ссылку на активацию учетной записи.<br />
        <br />
        Чтобы начать играть на нашем сайте необходимо активировать учетную запись!<br />
      </div>
      <template v-slot:controls>
        <button @click="reset">
          Понял, проверю почту!
        </button>
      </template>
    </lightbox>
  </div>
</template>

<script lang="ts">
import { Vue } from 'vue-property-decorator';
import Component from 'vue-class-component';
import ConfirmLightbox from '@/components/ConfirmLightbox.vue';
import accountApi from '@/api/requests/accountApi';

@Component({
  components: { ConfirmLightbox },
})
export default class Register extends Vue {
  private readonly successLightboxName = 'register-success';

  private email = '';
  private login = '';
  private password = '';

  private async register(): Promise<void> {
    const { email, login, password } = this;
    const { error } = await accountApi.register({ email, login, password });
    if (!error) {
      this.$modal.hide('register');
      this.$modal.show(this.successLightboxName);
    }
  }

  private reset(): void {
    this.email = '';
    this.login = '';
    this.password = '';
    this.$modal.hide(this.successLightboxName);
  }
}
</script>

<style lang="stylus" scoped>
</style>
