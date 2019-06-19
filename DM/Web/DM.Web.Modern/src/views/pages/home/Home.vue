<template>
  <div>
    <news />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import News from './News.vue';

@Component({
  components: {
    News,
  },
})
export default class HomePage extends Vue {}
</script>
