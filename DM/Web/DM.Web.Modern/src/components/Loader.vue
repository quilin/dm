<template>
  <div class="loader">&nbsp;</div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component({})
export default class Loader extends Vue {
}
</script>

<style scoped lang="stylus">
.loader
  square($medium)
  background-image url('~@/assets/loader.gif')
</style>
