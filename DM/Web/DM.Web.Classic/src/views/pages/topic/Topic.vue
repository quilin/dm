<template>
  <div>
    {{$route.params.id}}
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

export default class TopicPage extends Vue {
}
</script>