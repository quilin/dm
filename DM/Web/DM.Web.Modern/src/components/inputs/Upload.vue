<template>
  <input type="file" class="upload-input" ref="input" @change="upload" />
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component({})
export default class Upload extends Vue {
  public $refs!: {
    input: HTMLInputElement;
  }

  private upload(event: Event) {
    const target = event.target as HTMLInputElement;
    const files = target.files!;

    if (files.length === 0) return;

    const formData = new FormData();
    const name = files.length === 1 ? 'file' : 'files';

    for (let i = 0; i < files.length; ++i)
      formData.append(name, files[i]);

    this.$emit('uploading', formData);
  }
}
</script>

<style lang="stylus" scoped>
.upload-input
  position absolute
  bottom 0
  top 0
  left 0
  right 0
  width 100%
  padding 0
  margin 0
  opacity 0
  border none
  outline none
  cursor pointer
</style>