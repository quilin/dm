<template>
  <span class="tags__wrapper">
    <dropdown v-if="tags && tags.length" id="tags" class="tags__selector"
      v-model="selectedTag" :options="tagOptions" emptyLabel="Добавить..." />
    <loader v-else />
    <div v-if="value.length" class="tags__container">
      <span class="tag__container" v-for="tag in value" :key="tag.id">
        {{tag.title}}
        <a @click="removeTag(tag)"><icon :font="IconType.Close" /></a>
      </span>
    </div>
  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { cloneDeep } from 'lodash';
import { Tag } from '@/api/models/gaming';
import IconType from '@/components/iconType';

const defaultTagOption = {} as Tag;

@Component({})
export default class TagSelector extends Vue {
  private IconType: typeof IconType = IconType;
  private selectedTag: Tag = defaultTagOption;

  @Prop()
  private value!: Tag[];

  @Getter('gaming/tags')
  private tags!: Tag[] | null;

  @Action('gaming/fetchTags')
  private fetchTags!: any;

  @Watch('selectedTag')
  private onTagSelected(tag: Tag): void {
    if (tag === defaultTagOption) return;

    if (!this.value.some(t => t.id === tag.id)) {
      this.value.push(tag);
      const valueCopy = cloneDeep(this.value);
      this.$emit('input', valueCopy);
    }

    this.selectedTag = defaultTagOption;
  }

  private get tagOptions() {
    if (this.tags === null) return [];

    return this.tags.map((tag: Tag) => ({
      value: tag,
      label: tag.title,
      description: tag.category,
    }));
  }

  private removeTag(tag: Tag): void {
    const tagIndex = this.value.findIndex(t => t === tag);
    if (tagIndex === -1) return;

    const valueCopy = cloneDeep(this.value);
    valueCopy.splice(tagIndex, 1);

    this.$emit('input', valueCopy);
  }

  private mounted(): void {
    this.fetchTags();
  }
}
</script>

<style lang="stylus" scoped>
.tags__wrapper
  display flex
  flex-grow 1
  align-items baseline

.tags__selector
  min-width initial

.tags__container
  display flex
  flex-wrap wrap

.tag__container
  padding $inputPadding
  margin 0 0 $small $small

  themeExtend(border, 1px solid, $border)
  border-radius $borderRadius
  cursor default
  white-space nowrap
</style>