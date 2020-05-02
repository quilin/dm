<template>
  <Header>
    <template v-slot:title>DM.AM</template>
    <template v-slot:subtitle>Добро пожаловать!</template>
  </Header>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import Header from "./Header.vue";

  @Component({
    components: {
      Header,
    },
  })

  export default class HeaderDefault extends Vue {

  }
</script>
