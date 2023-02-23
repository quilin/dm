<template>
  <router-link
    class="rating"
    :to="{ name: 'rating', params: { login: props.user.login } }"
  >
    <template v-if="props.user.rating.enabled">
      <span
        class="quality"
        :class="{
          positive: props.user.rating.quality > 0,
          negative: props.user.rating.quality < 0,
        }"
        >{{ props.user.rating.quality }}</span
      >/{{ props.user.rating.quantity }}
    </template>
    <template v-else>скрыт</template>
  </router-link>
</template>

<script setup lang="ts">
import type { User } from "@/api/models/community";

const props = defineProps<{ user: User }>();
</script>

<style scoped lang="stylus">
.quality
  font-weight bold

.positive
  theme(color, $positiveText)

.negative
  theme(color, $negativeText)
</style>
