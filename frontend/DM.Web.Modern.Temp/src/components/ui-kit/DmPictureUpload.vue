<script setup lang="ts">
import { IconType } from "@/components/ui-kit/iconType";
import { computed, ref, watch } from "vue";
import DmUpload from "@/components/ui-kit/DmUpload.vue";
import DmIcon from "@/components/ui-kit/DmIcon.vue";
import DmProgress from "@/components/ui-kit/DmProgress.vue";

const props = defineProps<{
  canUpload: boolean;
  alt: string;
  progressEvent: ProgressEvent | null;
  pictureUrl: string | undefined;
}>();
const emit = defineEmits<{ (e: "uploadStarted", picture: FormData): void }>();

const loaded = ref(0);
const total = ref(0);
const progress = computed(() =>
  total.value > 0 ? Math.floor((loaded.value / total.value) * 100) : 0,
);

const uploading = computed(() => total.value > loaded.value);
const uploaded = computed(
  () => total.value > 0 && total.value === loaded.value,
);

watch(
  () => props.progressEvent,
  (progressEvent: ProgressEvent | null) => {
    if (progressEvent === null) {
      loaded.value = 0;
      total.value = 0;
      return;
    }

    loaded.value = progressEvent.loaded;
    total.value = progressEvent.total;

    if (loaded.value === total.value) {
      setTimeout(() => {
        loaded.value = 0;
        total.value = 0;
      }, 2000);
    }
  },
);
</script>

<template>
  <div class="picture_upload-container">
    <img :src="props.pictureUrl" class="picture_upload-picture" :alt="alt" />
    <template v-if="canUpload">
      <span
        :class="[
          'picture_upload-status',
          !uploading && !uploaded && 'picture_upload-status-ready',
        ]"
      >
        <template v-if="uploading">
          <dm-progress
            class="picture_upload-progress"
            :goal="total"
            :current="loaded"
            >{{ progress }}%</dm-progress
          >
        </template>
        <template v-else-if="uploaded">
          <dm-icon :font="IconType.Tick" />&nbsp;Изображение загружено
        </template>
        <template v-else>
          <dm-icon :font="IconType.Upload" />&nbsp;Загрузить изображение
        </template>
      </span>
      <dm-upload
        v-if="!uploading"
        @upload="(picture: FormData) => emit('uploadStarted', picture)"
      />
    </template>
  </div>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"
@use "@/assets/styles/Themes"

.picture_upload-container
  position: relative

.picture_upload-picture
  width: 100%
  max-height: Variables.$grid-step * 200
  border-radius: Variables.$border-radius

.picture_upload-status
  display: flex
  align-items: center
  justify-content: center

  position: absolute
  bottom: 0
  left: 0
  right: 0
  height: Variables.$grid-step * 10
  padding: 0 Variables.$small

  +Themes.theme(background-color, Themes.$shade-background)
  +Themes.theme(color, Themes.$shade-text)
  text-align: center

  border-bottom-left-radius: Variables.$border-radius
  border-bottom-right-radius: Variables.$border-radius

  &.picture_upload-status-ready
    opacity: 0

.picture_upload-container:hover .picture_upload-status-ready
  opacity: 1

.picture_upload-progress
  margin: 0
  flex-grow: 1
</style>
