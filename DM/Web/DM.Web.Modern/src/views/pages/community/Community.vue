<template>
  <div class="community-info">
    <div class="page-title">Сообщество</div>
    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action } from 'vuex-class';

@Component({})
export default class Community extends Vue {
  @Action('community/fetchUsers')
  private fetchUsers: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const { n } = this.$route.params;
    this.fetchUsers({ number: n, inactive: true });
  }
}
</script>