<template>
  <div>
    Notifications state:
    <span v-if="connection">{{ connection.state }}</span>
    <span v-else>Disconnected</span>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Api from '@/api';
import { HubConnection } from '@microsoft/signalr';

@Component({})
export default class NotificationsComponent extends Vue {
  private connection: HubConnection | null = null;

  private mounted() {
    this.connection = Api.establishHubConnection('whatsup');
    this.connection.on('send', data => {
      console.log(data);
    });
    this.connection.start();
  }

  private unmounted() {
    if (this.connection) {
      this.connection.stop();
    }
  }
}
</script>

<style lang="stylus"></style>