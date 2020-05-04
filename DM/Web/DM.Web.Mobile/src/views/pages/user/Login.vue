<template>
  <div name="login">
    <div v-if="user" class="login-already">
      <div>Вы уже вошли</div>
      <div><a class="button" href="javascript:void(0)" v-if="user" @click="signOut">Выйти</a></div>
    </div>
    <div v-else class="login-form">
      <h1>Вход</h1>
      <div class="form">
        <div class="form-block">
          <div class="form-field" :class="{ error: $v.login.$error }">
            <label class="form-field-label">Логин</label>
            <input class="form-input" type="text" v-model.trim="$v.login.$model"/>
          </div>
          <div v-if="$v.login.$error" class="form-field-error"></div>
        </div>
        <div class="form-block">
          <div class="form-field" :class="{ error: $v.password.$error }">
            <label class="form-field-label">Пароль</label>
            <input class="form-input" type="password" v-model="$v.password.$model"/>
          </div>
          <div v-if="$v.password.$error" class="form-field-error">$v.password.$error</div>
        </div>
        <div class="form-block">
          <div class="form-field">
            <label><input type="checkbox" v-model="rememberMe"/> Запомнить меня</label>
          </div>
        </div>
      </div>
      <div class="error" v-if="err.invalidProperties">
        <div class="error-title">{{err.message}}</div>
        <div v-for="prop in err.invalidProperties" :key="`prop-`+prop.id">
          <p class="error-msg" v-for="msg in prop" :key="`login-`+msg.id">{{msg}}</p>
        </div>
      </div>
      <div class="controls">
        <input type="button" @click="signIn" :disabled="$v.$error" value="Войти"/>
        <router-link class="button" :to="{name: 'UserRegister'}">Зарегистрироваться</router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import {Vue} from 'vue-property-decorator';
  import Component from 'vue-class-component';
  import {required} from 'vuelidate/lib/validators';
  import {Action, Getter} from 'vuex-class';
  import accountApi from '@/api/requests/accountApi';
  import {User} from "@/api/models/community";

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
    private login = '';
    private password = '';
    private rememberMe = false;
    private err: any = {};

    private validations: any = {
      login: {
        required,
      },
      password: {
        required,
      },
    };

    @Getter('user')
    private user!: User | null;

    @Action('signOut')
    private signOut: any;

    @Action('authenticate')
    private authenticate: any;

    private async signIn(): Promise<void> {
      const {login, password, rememberMe} = this;
      const {data, error} = await accountApi.signIn({login, password, rememberMe});
      if (error) {
        this.err = error;
      } else {
        this.authenticate(data!.resource).then(
          () => {
            this.$router.push({name: 'Home'});
          }
        );
      }
    }
  }
</script>

<style lang="stylus">
  .login-already
    margin $medium 0
    display flex
    flex-direction row
    align-items center
    justify-content space-between
  .error
    color red
    margin $medium 0

    .error-msg
      font-size $secondaryFontSize
      margin $small 0

  .form
    .form-block
      margin $medium 0

      .form-field
        display flex
        flex-direction row
        align-items center
        justify-content space-between

        .form-field-label
          flex 1

        .form-input
          flex 3

  .controls
    display grid
    grid-template-columns repeat(2, 1fr)
    grid-gap $small


</style>
