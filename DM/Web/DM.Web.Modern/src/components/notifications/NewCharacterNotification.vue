<template>
  <div>
    Игрок
    <router-link :to="{ name: 'profile', params: { login: data.AuthorLogin } }">{{ data.AuthorLogin }}</router-link>
    подал заявку в вашу игру
    <router-link :to="{ name: 'game', params: { id: data.GameId } }">
      {{ data.GameTitle }}
    </router-link>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { NewCharacterData } from '../../api/models/notifications';

@Component({})
export default class NewCharacterNotification extends Vue {
  @Prop()
  private data!: NewCharacterData;
}
</script>

<style lang="stylus">
</style>