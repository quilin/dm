<template>
  <div>
    Notifications state:
    <span v-if="connection">{{ connection.state }}</span>
    <span v-else>Disconnected</span>

    <portal to="notifications" v-if="burningNotifications.length">
      <div class="notification" v-for="notification in burningNotifications" :key="notification.id" @click.right.prevent="hide(notification.id)">
        <new-character-notification v-if="notification.eventType === NotificationType.NewCharacter" :data="notification.payload" />
        <topic-liked-notification v-else-if="notification.eventType === NotificationType.LikedTopic" :data="notification.payload" />
      </div>
    </portal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Api from '@/api';
import { HubConnection } from '@microsoft/signalr';
import { Action } from 'vuex-class';
import { NotificationType, UserNotification } from '@/api/models/notifications';
import NewCharacterNotification from '@/components/notifications/NewCharacterNotification.vue';
import TopicLikedNotification from '@/components/notifications/TopicLikedNotification.vue';

@Component({
  components: {TopicLikedNotification, NewCharacterNotification}
})
export default class NotificationsComponent extends Vue {
  private connection: HubConnection | null = null;
  private NotificationType: typeof NotificationType = NotificationType;

  private burningNotifications: UserNotification[] = [];

  @Action('notifications/push')
  private pushNotification: any;

  private hide(id: string) {
    const notificationIndex = this.burningNotifications.findIndex(n => n.id === id);
    console.log(notificationIndex, id);
    if (notificationIndex >= 0) {
      this.burningNotifications.splice(notificationIndex, 1);
    }
  }

  private mounted() {
    this.connection = Api.establishHubConnection('whatsup');
    this.connection.on('send', ((data: any) => {
      this.pushNotification(data);
      const notification = data as UserNotification;
      this.burningNotifications.unshift(notification);
      setTimeout(() => this.hide(notification.id), 150000);
    }).bind(this));
    this.connection.start();
  }

  private unmounted() {
    if (this.connection) {
      this.connection.stop();
    }
  }
}
</script>

<style lang="stylus">
.notification
  padding $medium
  margin $small $medium $medium
  border-radius $borderRadius
  theme(background-color, $notificationBackground)
  theme(color, $notificationText)
  cursor default

  & a
    theme(color, $notificationLink)
    &.router-link-exact-active
      theme(color, $notificationLink)
      font-weight normal
    &:hover
      theme(color, $notificationHoverLink)
</style>
