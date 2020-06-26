<template>
  <div>
    <active-polls />
    <donate />
    <most-read-games />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import ActivePolls from '@/views/layout/sidebar/ActivePolls.vue';
import Donate from '@/views/layout/sidebar/Donate.vue';
import MostReadGames from '@/views/layout/sidebar/MostReadGames.vue';

@Component({
  components: {
    ActivePolls,
    Donate,
    MostReadGames,
  },
})
export default class GeneralMenu extends Vue {}
</script>
