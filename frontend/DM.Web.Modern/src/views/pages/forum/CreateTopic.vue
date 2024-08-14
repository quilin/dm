<template>
  <div>
    <button @click="$modal.show('create-topic')">
      <icon :font="IconType.Add" />
      Создать тему
    </button>

    <confirm-lightbox
        name="create-topic"
        title="Создать тему"
        accept-text="Создать"
        :accept-disabled="formEmpty"
        @accepted="createTopic"
        @canceled="$modal.hide('create-topic')"
    >
      <div class="form">
        <label class="field-label create-topic__label">Название</label>
        <input type="text" v-model="title" />
        <label class="field-label create-topic__label create-topic__description">Описание</label>
        <text-area v-model="description" />
      </div>
    </confirm-lightbox>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import IconType from '@/components/iconType';
import ConfirmLightbox from '@/components/ConfirmLightbox.vue';

@Component({
  components: { ConfirmLightbox },
})
export default class CreateTopicComponent extends Vue {
  private IconType: typeof IconType = IconType;

  private title = '';
  private description = '';

  private get formEmpty() {
    return this.title === '' || this.description === '';
  }

  @Getter('forum/selectedForum')
  private selectedForum!: string;

  @Action('forum/createTopic')
  private createTopicAction: any;

  private createTopic() {
    this.createTopicAction({
      topic: {
        forum: {
          id: this.selectedForum,
          unreadTopicsCount: 0,
        },
        title: this.title,
        description: this.description,
      },
      router: this.$router,
    });
  }
}
</script>

<style lang="stylus">
  .create-topic {
    &__label {
      display block;

      margin-bottom $minor
    }

    &__description {
      margin-top $small;
    }
  }
</style>
