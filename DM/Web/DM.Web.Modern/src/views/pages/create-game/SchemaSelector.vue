<template>
  <span>
    <dropdown v-if="schemas && schemas.length" id="attribute-schema"
      :value="value" @input="changeSchema" :options="schemasList" />
    <loader v-else />

    <create-schema @created="changeSchema" />
  </span>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { AttributeSchema, AttributeSchemaType } from '@/api/models/gaming';

import CreateSchema from '@/views/pages/create-game/CreateSchema.vue';

const createSchemaOption = {} as AttributeSchema;

@Component({
  components: {
    CreateSchema,
  },
})
export default class SchemaSelector extends Vue {
  @Prop()
  private value!: AttributeSchema;

  @Getter('gaming/schemas')
  private schemas!: AttributeSchema[] | null;

  @Action('gaming/fetchSchemas')
  private fetchSchemas!: any;

  private changeSchema(schema: AttributeSchema): void {
    if (schema === createSchemaOption) {
      this.$modal.show('create-schema');
    } else {
      this.$emit('input', schema);
    }
  }

  private get schemasList() {
    if (this.schemas === null) return [];

    return [{
      value: null as AttributeSchema | null,
      label: 'не нужны',
    }].concat(this.schemas!.map((s: AttributeSchema) => ({
      value: s,
      label: s.title,
      description: s!.type === AttributeSchemaType.Public ? 'публичная схема' : `автор: ${s.author!.login}`,
    }))).concat([{
      value: createSchemaOption,
      label: 'Создать новую схему...',
    }]);
  }

  private mounted(): void {
    this.fetchData();
  }

  private fetchData(): void {
    this.fetchSchemas();
  }
}
</script>

<style lang="stylus" scoped>
</style>