<template>
  <div>
    <div class="title">
       <icon :font="IconType.Ruble" /> Помочь проекту
    </div>
    <progress-bar :current="current" :goal="goal">{{current}} / {{goal}} р.</progress-bar>
    dm.am &ndash; некоммерческий проект.<br />
    <router-link :to="{ name: 'donate' }">Помогите нам</router-link> хотя бы не испортить его!
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import IconType from '@/components/iconType';
import ProgressBar from '@/components/ProgressBar.vue';

@Component({
  components: {
    ProgressBar,
  }
})
export default class Donate extends Vue {
  private IconType: typeof IconType = IconType;

  private current = 24332;
  private goal = 60000;
}
</script>

<style scoped lang="stylus">
.title
  header()
</style>
