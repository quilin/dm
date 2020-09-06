<template>
  <lightbox name="create-schema" width="600">
    <template slot="title">Новая схема характеристик</template>

    <form @submit.prevent="save">
      <div class="form-field">
        <input id="schema-title" v-model="schema.title" placeholder="Введите название" />
      </div>

      <div class="specifications-title">Характеристики</div>
      <draggable :list="schema.specifications" handle=".handle">
        <div v-for="(specification, index) in schema.specifications" :key="index" class="specification-container">
          <div class="specification-actions">
            <a class="handle"><icon :font="IconType.Reorder" /></a>
            <a @click="removeSpecification(index)"><icon :font="IconType.Close" /></a>
          </div>
          <div class="form-field">
            <span :for="`spec-title-${index}`">{{index + 1}}.</span>
            <input :id="`spec-title-${index}`" v-model="specification.title" placeholder="Введите название" />
            <dropdown v-model="specification.type" :options="constraintOptions" />
            <label v-if="specification.type !== AttributeSpecificationType.List">
              <input type="checkbox" v-model="specification.required" />
              обязательно
            </label>
          </div>
          <div class="specification-constraints">
            <div class="specification-constraints-toggle">Ограничения</div>

            <div v-if="specification.type === AttributeSpecificationType.Number" class="form-field">
              <span :for="`spec-number-constraints-from-${index}`">Диапазон значений от</span>
              <input type="number" :id="`spec-number-constraints-from-${index}`"
                     v-model.number="specification.minValue"
                     placeholder="–∞"
                     size="5" />
              <span :fot="`spec-number-constraints-to-${index}`">до</span>
              <input type="number" :id="`spec-number-constraints-to-${index}`"
                     v-model.number="specification.maxValue"
                     placeholder="+∞"
                     size="5" />
            </div>

            <div v-else-if="specification.type === AttributeSpecificationType.String" class="form-field">
              <label :for="`spec-string-constraints-${index}`">Максимальная длина</label>
              <input type="number" :id="`spec-string-constraints-${index}`"
                     v-model.number="specification.maxLength"
                     size="5" />
            </div>

            <template v-else>
              <draggable v-if="specification.values && specification.values.length"
                         :list="specification.values" handle=".values-handle" ghost-class="specification-constraints-ghost">
                <div v-for="(value, valueIndex) in specification.values"
                     :key="`${specification.title}_${valueIndex}`" class="form-field">
                  <span>{{valueIndex + 1}}.</span>
                  <input v-model="value.value" placeholder="Введите название" />
                  <input type="number" v-model.number="value.modifier" size="3" />
                  <span>
                  <a class="values-handle"><icon :font="IconType.Reorder" /></a>
                  <a @click="removeValue(specification, valueIndex)"><icon :font="IconType.Close" /></a>
                </span>
                </div>
              </draggable>
              <a @click="addValue(specification)" class="specification-constraints-addValue">
                <icon :font="IconType.Add" /> добавить значение
              </a>
            </template>
          </div>
        </div>
      </draggable>

      <a @click="addSpecification">
        <icon :font="IconType.Add" />
        добавить характеристику
      </a>
    </form>

    <template slot="controls">
      <action-button :loading="saving" @click="save">Создать схему</action-button>
    </template>
  </lightbox>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Action } from 'vuex-class';

import {
  AttributeSchema,
  AttributeSchemaType,
  AttributeSpecification,
  AttributeSpecificationType
} from '@/api/models/gaming';
import IconType from '@/components/iconType';
import draggable from 'vuedraggable';

const constraintOptions = [{
  value: AttributeSpecificationType.Number,
  label: 'число',
}, {
  value: AttributeSpecificationType.String,
  label: 'строка',
}, {
  value: AttributeSpecificationType.List,
  label: 'список',
}];

const defaultSchema = {
  id: null,
  author: null,
  title: '',
  specifications: [],
  type: AttributeSchemaType.Private,
};

@Component({
  components: {
    draggable,
  }
})
export default class CreateSchema extends Vue {
  private IconType: typeof IconType = IconType;
  private AttributeSpecificationType: typeof AttributeSpecificationType = AttributeSpecificationType;

  private constraintOptions = constraintOptions;
  private schema: AttributeSchema = defaultSchema as AttributeSchema;
  private saving = false;

  @Action('gaming/createSchema')
  private createSchema!: any;

  private addSpecification() {
    this.schema.specifications.push({
      title: '',
      type: AttributeSpecificationType.Number,
      required: false,
      values: [{
        value: 'Плохо',
        modifier: -5,
      }, {
        value: 'Обычно',
        modifier: 0,
      }, {
        value: 'Хорошо',
        modifier: 5,
      }],
    } as AttributeSpecification)
  }

  private removeSpecification(index: number) {
    this.schema.specifications.splice(index, 1);
  }

  private addValue(specification: AttributeSpecification) {
    if (specification.values === null) {
      specification.values = [];
    }
    specification.values!.push({
      value: '',
      modifier: 0,
    });
  }

  private removeValue(specification: AttributeSpecification, index: number) {
    specification.values!.splice(index, 1);
  }

  private async save(): Promise<void> {
    this.saving = true;
    const schema = await this.createSchema({ schema: this.schema, $router: this.$router });
    this.$emit('created', schema);
    this.saving = false;
    this.$modal.hide('create-schema');
  }
}
</script>

<style lang="stylus" scoped>
input[type="number"]
  width $gridStep * 10

.form-field > *
  margin-right $small

.specifications-title
  header()

.specification-container
  position relative
  margin-bottom $big
  border-radius $borderRadius

.specification-actions
  position absolute
  top $small
  right $small

.specification-constraints
  padding 0 $medium

.specification-constraints-toggle
  margin $small $small 0

  theme(background-color, $background)
  secondary()

.specification-constraints-ghost
  theme(background-color, $panelBackground)
  opacity 0.8

.specification-constraints-addValue
  display inline-block
  margin-bottom $small
</style>
