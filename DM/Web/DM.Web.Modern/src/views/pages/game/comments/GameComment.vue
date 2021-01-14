<template>
  <div>
    {{ comment.text }}
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Comment } from '@/api/models/gaming';

@Component({})
export default class GameComment extends Vue {
  @Prop()
  private comment!: Comment
}
</script>