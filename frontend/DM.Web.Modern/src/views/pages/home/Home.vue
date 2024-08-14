<template>
  <div>

    <news />

    <random-review />

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import News from './News.vue';
import RandomReview from './RandomReview.vue';

@Component({
  components: {
    News,
    RandomReview,
  },
})
export default class HomePage extends Vue {}
</script>
