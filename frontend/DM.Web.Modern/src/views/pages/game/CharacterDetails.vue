<template>
  <div class="character">
    <div class="preview" ref="preview">

      <div class="info-container">
        <div class="picture-container">
          <div class="picture" :style="{ backgroundImage: character.pictureUrl }" />
          <span :class="`status-${statuses[character.status].displayClass}`">
            {{ statuses[character.status].displayText }}
          </span>
        </div>
        <div class="info">
          <div class="info-main">{{ character.name }}</div>
          <div class="info-additional">
            {{ [character.race, character.class].filter(c => c).join(' / ') }}
          </div>

          <div class="details">
            <template v-if="character.attributes && character.attributes.length">
              <div class="details-block">Характеристики</div>
              <div class="details-attribute" v-for="attribute in character.attributes" :key="attribute.id">
                <div class="details-attribute-label">{{ attribute.title }}</div>
                <div class="details-attribute-value">
                  {{ attribute.value }}
                  <icon v-if="attribute.inconsistent" :font="IconType.Blast" class="details-attribute-alert" />
                </div>
                <div v-if="attribute.modifier" class="details-attribute-value">{{ attribute.modifier }}</div>
              </div>
            </template>

            <template v-if="character.appearance">
              <div class="details-block">Внешность</div>
              {{ character.appearance }}
            </template>
            <template v-if="character.temper">
              <div class="details-block">
                Характер
                <icon v-if="privacySettings.viewTemper" :font="IconType.Hidden" />
              </div>
              {{ character.temper }}
            </template>
            <template v-if="character.story">
              <div class="details-block">
                История
                <icon v-if="privacySettings.viewStory" :font="IconType.Hidden" />
              </div>
              {{ character.story }}
            </template>
            <template v-if="character.skills">
              <div class="details-block">
                Навыки
                <icon v-if="privacySettings.viewSkills" :font="IconType.Hidden" />
              </div>
              {{ character.skills }}
            </template>
            <template v-if="character.inventory">
              <div class="details-block">
                Инвентарь
                <icon v-if="privacySettings.viewInventory" :font="IconType.Hidden" />
              </div>
              {{ character.inventory }}
            </template>
          </div>

        </div>
      </div>

      <character-actions v-if="user" :character="character" />

    </div>

    <div class="expand-link" @click="toggle">
      <icon :font="expanded ? IconType.CornerTop : IconType.CornerBottom" />
      <span class="expand-link-text">{{ expanded ? 'скрыть' : 'развернуть' }}</span>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Getter } from 'vuex-class';
import { Character, CharacterStatus, GamePrivacySettings } from '@/api/models/gaming';
import { User } from '@/api/models/community';
import IconType from '@/components/iconType';

import CharacterActions from '@/views/pages/game/CharacterActions.vue';

const characterStatuses: { [key: string]: { displayText: string; displayClass: string } } = {
  [CharacterStatus.Registration]: { displayText: 'Заявка', displayClass: 'neutral' },
  [CharacterStatus.Active]: { displayText: 'В игре', displayClass: 'positive' },
  [CharacterStatus.Declined]: { displayText: 'Отклонен', displayClass: 'negative' },
  [CharacterStatus.Left]: { displayText: 'Вышел', displayClass: 'neutral' },
  [CharacterStatus.Dead]: { displayText: 'Мертв', displayClass: 'negative' },
};

@Component({
  components: { CharacterActions }
})
export default class CharacterDetails extends Vue {
  public $refs!: {
    preview: HTMLElement;
  };

  private IconType: typeof IconType = IconType;
  private statuses = characterStatuses;
  private expanded = false;

  @Prop()
  private character!: Character;

  @Prop()
  private privacySettings!: GamePrivacySettings;

  @Getter('user')
  private user!: User | null;

  private toggle() {
    this.expanded = !this.expanded;
    const details = this.$refs.preview;
    if (this.expanded) {
      details.style.height = 'auto';
      const neededHeight = details.clientHeight;
      details.style.height = '108px';
      setTimeout(() => details.style.height = `${neededHeight}px`, 0);
      setTimeout(() => details.style.height = 'auto', 200);
    } else {
      details.style.height = `${details.clientHeight}px`;
      setTimeout(() => details.style.height = '108px', 0);
    }
  }
}
</script>

<style lang="stylus">
.character
  padding $medium

.preview
  position relative
  display flex
  justify-content space-between
  height $gridStep * 27
  overflow hidden
  transition height $animationTime

  &:after
    content ''
    position absolute
    bottom 0
    left 0
    right 0
    themeExtend(box-shadow, 0 0 $small $small, $background)

.picture-container
  margin-right $medium
  text-align center

.picture
  width $gridStep * 20
  height @width
  border-radius @width
  themeExtend(box-shadow, inset 0 0 $minor, $border)
  background url('~@/assets/userpic.png') 0 0 no-repeat
  background-size contain

.status-positive
  theme(color, $positiveText)

.status-negative
  theme(color, $negativeText)

.info-container
  display flex
  justify-content space-between

.info-main
  font-size $medium

.info-additional
  secondary()

$attributeGridTemplate = \[label\] 25% \[value\] 25% \[modifier\] 10%
.details-attribute
  grid($attributeGridTemplate)

.details-attribute-label
  theme(color, $secondaryText)

.details-attribute-alert
  theme(color, $negativeText)

.details
  padding-bottom $small

.details-block
  margin $medium 0 $small
  font-weight bold

.expand-link
  font-size $titleFontSize
  text-align center
  transition opacity $animationTime
  cursor pointer
  opacity 0.5

  &:hover
    opacity 1
    themeExtend(box-shadow, 0 $small ($gridStep * 3) (-($small)), $border)

  & > *
    vertical-align middle

.expand-link-text
  font-size $secondaryFontSize
</style>