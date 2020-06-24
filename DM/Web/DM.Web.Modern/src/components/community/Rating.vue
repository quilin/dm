<template>
  <router-link class="rating" :to="{ name: 'rating', params: { login: user.login } }">
    <template v-if="user.rating.enabled">
      <span class="quality" :class="{
        positive: user.rating.quality > 0,
        negative: user.rating.quality < 0,
      }">{{user.rating.quality}}</span>/{{user.rating.quantity}}
    </template>
    <template v-else>скрыт</template>
  </router-link>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { User } from '@/api/models/community';

@Component({})
export default class Rating extends Vue {
  @Prop()
  private user!: User;
}
</script>

<style scoped lang="stylus">
.quality
  font-weight bold

.positive
  theme(color, $positiveText)

.negative
  theme(color, $negativeText)
</style>