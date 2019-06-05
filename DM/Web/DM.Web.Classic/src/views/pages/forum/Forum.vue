<template>
  <div>
    <div class="title">{{$route.params.id}} | Форум</div>
    <div class="moderators"></div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { mapGetters } from 'vuex';

@Component({
  computed: {
    ...mapGetters({
      moderators: 'currentForumModerators',
    }),
  },
})
export default class Forum extends Vue {
  private mounted(): void {
    this.$store.dispatch('selectForum', { id: this.$route.params.id });
    this.$store.dispatch('fetchCurrentForumModerators');
  }
}
</script>

<style scoped lang="stylus">
.title
  pageTitle()

.moderators
  header()
</style>
