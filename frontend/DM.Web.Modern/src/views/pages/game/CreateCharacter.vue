<template>
  <div>

    <loader v-if="!game" :big="true" />

    <form v-else @submit.prevent="create">
      <div class="page-title">Создание персонажа</div>

      <div class="description">
        <div class="form-field">
          <label for="name">Имя</label>
          <span id="name">
            <input v-model="character.name" />
          </span>
        </div>
        <div class="form-field">
          <label for="race">Раса</label>
          <span id="race">
            <input v-model="character.race" />
          </span>
        </div>
        <div class="form-field">
          <label for="class">Класс</label>
          <span id="class">
            <input v-model="character.class" />
          </span>
        </div>
      </div>

      <div class="field" v-if="game.schema && character.attributes && character.attributes.length">
        <div v-for="(specification, index) in game.schema.specifications" :key="specification.id" class="form-field">
          <label :for="`attribute_${index}`">{{specification.title}}</label>
          <input v-if="specification.type === AttributeSpecificationType.Number" v-model="character.attributes[index].value" type="number"
            :max="specification.maxValue"
            :min="specification.minValue"
            :size="Math.max((specification.minValue + '').length, (specification.maxValue + '').length)" />
          <input v-else-if="specification.type === AttributeSpecificationType.String" v-model="character.attributes[index].value"
            :size="specification.maxLength"
            :maxlength="specification.maxLength" />
          <dropdown v-else-if="specification.type === AttributeSpecificationType.List" v-model="character.attributes[index].value"
            :options="specification.values.map(v => ({ value: v.value, label: v.value }))" />
        </div>
      </div>

      <div class="field">
        <label class="field-title">Внешность</label>
        <text-area v-model="character.appearance" />
      </div>

      <div class="field">
        <span class="description-title">Характер <icon v-if="!game.privacySettings.viewTemper" :font="IconType.Hidden" /></span>
        <text-area v-model="character.temper" />
      </div>

      <div class="field">
        <span class="field-title">История <icon v-if="!game.privacySettings.viewStory" :font="IconType.Hidden" /></span>
        <text-area v-model="character.story" />
      </div>

      <div class="field">
        <span class="field-title">Навыки <icon v-if="!game.privacySettings.viewSkills" :font="IconType.Hidden" /></span>
        <text-area v-model="character.skills" />
      </div>

      <div class="field">
        <span class="field-title">Инвентарь <icon v-if="!game.privacySettings.viewInventory" :font="IconType.Hidden" /></span>
        <text-area v-model="character.inventory" />
      </div>

      <action-button type="submit" :loading="creating">Подать заявку</action-button>
    </form>

  </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import IconType from '@/components/iconType';
import { Action, Getter } from 'vuex-class';
import {
  Alignment,
  AttributeSpecificationType,
  Character,
  CharacterAttribute,
  Game,
  GameParticipation
} from '@/api/models/gaming';

@Component({})
export default class CreateCharacterPage extends Vue {
  private IconType: typeof IconType = IconType;
  private AttributeSpecificationType: typeof AttributeSpecificationType = AttributeSpecificationType;
  private creating = false;

  private character: Character = {
    name: '',
    race: '',
    class: '',
    alignment: Alignment.TrueNeutral,
    appearance: '',
    temper: '',
    story: '',
    skills: '',
    inventory: '',
    privacy: {
      isNpc: false,
      editByMaster: false,
      editPostByMaster: false,
    },
    attributes: [],
  } as unknown as Character;

  @Action('gaming/createCharacter')
  private createCharacter: any;

  @Getter('gaming/selectedGame')
  private game!: Game;

  @Watch('game')
  private onGameChanged() {
    if (!this.game) return;

    this.character.attributes = this.game.schema!.specifications.map(spec => ({
      id: spec.id,
      title: spec.title,
      value: spec.type === AttributeSpecificationType.List
        ? spec.values![0].value
        : '',
    } as CharacterAttribute));
    this.character.privacy.isNpc = this.game.participation.some(p => p === GameParticipation.Authority)
  }

  private async create() {
    await this.createCharacter({ character: this.character, $router: this.$router });
  }

  private mounted() {
    this.onGameChanged();
  }
}
</script>

<style scoped lang="stylus">
.field
  margin $medium 0

.field-title
  theme(color, $secondaryText)
</style>