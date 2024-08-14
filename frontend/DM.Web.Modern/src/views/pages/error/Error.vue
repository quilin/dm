<template>
  <div>
    <div class="page-title">{{error.title}}</div>
    <div class="error-disclaimer">Случилось ужасное!</div>
    <div v-html="error.description" class="error-disclaimer"></div>
    <img class="error-image" :src="`/${$route.params.code}.png`" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

interface ErrorData {
  title: string;
  description: string;
}

const errors: { [key: number]: ErrorData } = {
  [400]: {
    title: 'Плохой запрос',
    description: 'Скорее всего, мы продолбали проверку отправляемых вами данных, и вы случайно отправили что-то неправильное.<br/>' +
        'Нам очень жаль, правда!'
  },
  [401]: {
    title: 'Сессия неактивна',
    description: 'Вы или слишком долго ничего не делали и ваша сессия оказалась просрочена, или вы нажали кнопку "выйти с других устройств".<br/>' +
        'Вам придется повторно войти на сайт, используя свой пароль.'
  },
  [403]: {
    title: 'Доступ ограничен',
    description: 'Вы попробовали совершить действие, на которое у вас нет разрешения.<br/>' +
      'Скорее всего, вы просто забыли войти на сайт. ' +
      'Ну или вы злоумышленник, и за вами уже выхали, так и знайте!',
  },
  [404]: {
    title: 'Страницы не существует',
    description: 'Возможно, вы допустили опечатку в адресе страницы или смеха ради написали там белиберду.',
  },
  [410]: {
    title: 'Содержимое отсутствует',
    description: 'Содержимое, которое вы пытаетесь посмотреть, больше не существует или не существовало никогда.'
  },
  [500]: {
    title: 'Ошибка на сервере',
    description: 'Это наш косяк. Но вы не переживайте, мы уже знаем об этой ошибке ' +
      'и предпринимаем все возможные меры. Кстати, иногда помогает попробовать еще раз.',
  },
};

@Component({})
export default class ErrorComponent extends Vue {
  private get error(): ErrorData {
    return errors[parseInt(this.$route.params.code, 10)];
  }

  private get url(): string {
    return `@/assets/${this.$route.params.code}.png`;
  }
}
</script>

<style lang="stylus" scoped>
.error-disclaimer
  margin $medium 0
  font-size $medium

.error-image
  display block
  margin $small auto
  width $gridStep * 100
  theme(filter, colorPair(none, invert(87%)))
</style>
