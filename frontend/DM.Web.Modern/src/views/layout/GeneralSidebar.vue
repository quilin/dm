<template>
  <div>
    <active-polls />
    <donate />
    <popular-games />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import ActivePolls from '@/views/layout/sidebar/ActivePolls.vue';
import Donate from '@/views/layout/sidebar/Donate.vue';
import PopularGames from '@/views/layout/sidebar/PopularGames.vue';

@Component({
  components: {
    ActivePolls,
    Donate,
    PopularGames,
  },
})
export default class GeneralMenu extends Vue {}
</script>
