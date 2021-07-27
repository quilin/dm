<template>
  <menu-block token="Fora">
    <template v-slot:title>Форумы</template>
    <loader v-if="!fora.length" />
    <div v-else v-for="forum in fora" :key="forum.id" class="menu-item"
      :class="{ selected: activeForumRoute && forum.id === selectedForumId }">
      <router-link :to="{name: 'forum', params: {id: forum.id, n: 1}}">
        {{forum.id}}
        <template v-if="forum.unreadTopicsCount">
          <icon :font="IconType.CommentsUnread" />
          {{forum.unreadTopicsCount}}
        </template>
      </router-link>
    </div>
  </menu-block>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';

import { User } from '@/api/models/community';
import { Forum } from '@/api/models/forum';
import IconType from '@/components/iconType';
import MenuBlock from '../MenuBlock.vue';

@Component({
  components: {
    MenuBlock,
  },
})
export default class ForaList extends Vue {
  private IconType: typeof IconType = IconType;

  @Getter('user')
  private user!: User;

  @Getter('forum/fora')
  private fora!: Forum[];

  @Getter('forum/selectedForum')
  private selectedForum!: string | null;

  private get selectedForumId() {
    return this.selectedForum;
  }

  private get activeForumRoute() {
    const name = this.$route.name;

    return name === 'forum' || name === 'topic' || name === 'topics';
  }

  @Action('forum/fetchFora')
  private fetchFora: any;

  @Watch('user')
  private onUserChange() {
    this.fetchFora();
  }

  private mounted() {
    this.fetchFora();
  }
}
</script>

<style scoped lang="stylus">
  .selected a
    font-weight bold
    theme(color, $text)
</style>
