<template>
  <div>
    <loader v-if="success === null" />
    <template v-else>
      <div class="page-title">Учетная запись успешно активирована!</div>
      Теперь вы можете <a @click="$modal.show('login')">войти</a> в систему и начать играть!
    </template>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import accountApi from '@/api/requests/accountApi';

@Component({})
export default class Activate extends Vue {
  private success: boolean | null = null;

  private async mounted() {
    const { error } = await accountApi.activate(this.$route.params.token);
    this.success = !error;
    if (error) {
      this.$router.push(`/error/${error.code}`);
    }
  }
}
</script>

<style lang="stylus"></style>
