<template>
  <div>
    Игрок
    <router-link :to="{ name: 'profile', params: { login: data.authorLogin } }">{{ data.authorLogin }}</router-link>
    подал заявку в вашу игру
    <router-link :to="{ name: 'game', params: { id: data.gameId } }">
      {{ data.gameTitle }}
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
