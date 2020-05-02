<template>
  <Header>
    <template v-slot:title>{{selectedForum}}</template>
    <template v-slot:subtitle>Форум</template>
  </Header>
</template>

<script lang="ts">
  import {Component, Watch, Vue} from 'vue-property-decorator';
  import { Action, Getter } from 'vuex-class';
  import { Route } from 'vue-router';
  import Header from "./Header.vue";
  import { User } from '@/api/models/community';

  const namespace = 'forum';

  @Component({
    components: {
      Header,
    },
  })

  export default class HeaderDefault extends Vue {
    @Getter('forum/selectedForum')
    private selectedForum!: string | null;

    @Action('selectForum', {namespace})
    private selectForum: any;

    @Watch('$route')
    private onRouteChanged(newValue: Route, oldValue: Route): void {
      if (newValue.params.id !== oldValue.params.id) {
        this.fetchData();
      }
    }

    private mounted(): void {
      this.fetchData();
    }

    private fetchData(): void {
      const id = this.$route.params.id;
      this.selectForum({ id, router: this.$router });
    }
  }
</script>
