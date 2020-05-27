<template>
  <div>
    <button @click="$modal.show('create-topic')">
      <icon :font="IconType.Add" />
      Создать тему
    </button>

    <lightbox name="create-topic">
      <template slot="title">Создать тему</template>
      <div class="form">
        <label class="field-label create-topic__label">Название</label>
        <input type="text" v-model="title" />
        <label class="field-label create-topic__label create-topic__description">Описание</label>
        <text-area v-model="description" />
      </div>
      <template slot="controls">
        <input type="button"
               @click="createTopic"
               :disabled="formEmpty"
               value="Создать" />
        <a @click="$modal.hide('create-topic')">Отменить</a>
      </template>
    </lightbox>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import {Action, Getter} from 'vuex-class';
import IconType from '@/components/iconType';


@Component({})
export default class CreateTopicComponent extends Vue {
  private IconType: typeof IconType = IconType;

  private title: string = '';
  private description: string = '';

  private get formEmpty(): boolean {
    return this.title === '' && this.description === '';
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
      router: this.$router
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
