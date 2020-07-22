<template>
  <div>
    <div class="page-title">Наши пользователи о нас</div>
    <router-view />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator';
import { User } from '@/api/models/community';
import { Action, Getter } from 'vuex-class';

@Component({})
export default class Reviews extends Vue {
  @Getter('user')
  private user!: User | null;

  @Action('community/fetchReviews')
  private fetchReviews: any;

  @Watch('$route')
  private onRouteChanged(): void {
    this.fetchData();
  }

  @Watch('user')
  private onUserChanged(): void {
    this.fetchData();
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    const { n } = this.$route.params;
    this.fetchReviews({ n });
  }
}
</script>

<style lang="stylus" scoped>

</style>