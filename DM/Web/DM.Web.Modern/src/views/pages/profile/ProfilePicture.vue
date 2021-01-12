<template>
  <div class="container">
    <span class="disclaimer" v-if="!uploaded && !uploading">
      <icon :font="IconType.Upload" /> Загрузить изображение
    </span>
    <span class="status" v-else>
      <template v-if="uploaded">
        <icon :font="IconType.Tick" /> Изображение загружено
      </template>
      <progress-bar class="progress" v-else :current="loaded" :goal="total">{{ progress }}%</progress-bar>
    </span>
    <upload @uploading="upload" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import IconType from '@/components/iconType';
import ProgressBar from '@/components/ProgressBar.vue';
import { Action } from 'vuex-class';

@Component({
  components: {ProgressBar}
})
export default class ProfilePicture extends Vue {
  private IconType: typeof IconType = IconType;
  private uploading = false;
  private uploaded = false;

  private loaded = 0;
  private total = 1;

  @Action('community/uploadProfilePicture')
  private uploadProfilePicture: any;

  private get progress() {
    return Math.floor(this.loaded / this.total * 100);
  }

  private async upload(formData: FormData): Promise<void> {
    this.uploadProfilePicture({ file: formData, progressCallback: this.onUploadProgress });
  }

  private onUploadProgress(progressEvent: ProgressEvent): void {
    this.loaded = progressEvent.loaded;
    this.total = progressEvent.total;

    this.uploading = progressEvent.loaded !== progressEvent.total;
    this.uploaded = !this.uploading;

    if (this.uploaded) {
      setTimeout(() => this.uploaded = false, 2000);
    }
  }
}
</script>

<style lang="stylus" scoped>
.picture-container
  position absolute
  top 0
  bottom 0
  left 0
  right 0

pictureUploadLabel()
  position absolute
  bottom 0
  left 0
  right 0
  padding $small
  theme(background-color, $shadeBackground);
  theme(color, $shadeText);
  text-align center

.disclaimer
  pictureUploadLabel()
  opacity 0
  transition opacity $animationTime

  .picture-container:hover &
    opacity 1

.status
  pictureUploadLabel()

.progress
  margin 0
</style>