<template>
  <div>
    Пользователю
    <router-link :to="{ name: 'profile', params: { login: data.authorLogin } }">{{ data.authorLogin }}</router-link>
    понравилась ваша тема на форуме
    <router-link :to="{ name: 'topic', params: { id: data.topicId } }">
      {{ data.topicTitle }}
    </router-link>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { TopicLikedData } from '../../api/models/notifications';

@Component({})
export default class TopicLikedNotification extends Vue {
  @Prop()
  private data!: TopicLikedData;
}
</script>

<style lang="stylus">
</style>
