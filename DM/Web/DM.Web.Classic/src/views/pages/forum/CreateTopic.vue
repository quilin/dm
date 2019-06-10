<template>
  <div>
    <button @click="$modal.show('create-topic')">
      <icon :font="IconType.Add" />
      Создать тему
    </button>

    <lightbox name="create-topic">
      <template slot="title">Создать тему</template>
      <div class="form">
        <label class="field-label">Название</label>
        <input type="text" v-model="title" />
        <label class="field-label">Описание</label>
        <text-area v-model="description" />
      </div>
      <template slot="controls">
        <input type="button" @click="create" :disabled="formEmpty" value="Создать" />
        <a href="javascript:void(0)" @click="$modal.hide('create-topic')">Отменить</a>
      </template>
    </lightbox>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import IconType from '@/components/iconType';

@Component({})
export default class CreateTopicComponent extends Vue {
  private IconType: typeof IconType = IconType;

  private title: string = '';
  private description: string = '';

  private get formEmpty(): boolean {
    return this.title === '' && this.description === '';
  }

  private async create(): Promise<void> {
    const x = 10;
  }
}
</script>

<style lang="stylus">

<style>
