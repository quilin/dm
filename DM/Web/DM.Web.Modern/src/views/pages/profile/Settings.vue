<template>

  <form @submit.prevent="save">
    <div class="settings-block">
      <span class="settings-title">Количество сообщений на странице</span>
      <div class="settings-per-page">
        <suggest-input v-model.number="user.settings.paging.postsPerPage" :suggestions="pagingSuggestions" size="3" maxlength="3" />
        &ndash; в играх
      </div>
      <div class="settings-per-page">
        <suggest-input v-model.number="user.settings.paging.commentsPerPage" :suggestions="pagingSuggestions" size="3" maxlength="3" />
        &ndash; в обсуждениях, новостях и на форуме
      </div>
      <div class="settings-per-page">
        <suggest-input v-model.number="user.settings.paging.messagesPerPage" :suggestions="pagingSuggestions" size="3" maxlength="3" />
        &ndash; в личных сообщениях
      </div>
    </div>
    <div class="settings-block">
      <span class="settings-title">Количество тем на странице форума</span>
      &ndash;
      <suggest-input v-model.number="user.settings.paging.topicsPerPage" :suggestions="pagingSuggestions" size="3" maxlength="3" />
    </div>
    <div class="settings-block">
      <span class="settings-title">Размер списков игр, пользователей и прочих</span>
      &ndash;
      <suggest-input v-model.number="user.settings.paging.entitiesPerPage" :suggestions="pagingSuggestions" size="3" maxlength="3" />
    </div>

    <div class="settings-block">
      <span class="settings-title">Цветовая схема</span>
      &ndash;
      <dropdown v-model="user.settings.colorSchema" :options="colorSchemaOptions" />
    </div>

    <div v-if="userIsNanny" class="settings-block">
      Приветственное письмо от няни
      <text-area v-model="user.settings.nannyGreetingsMessage" />
    </div>

    <action-button type="submit" :loading="saving" :disabled="unchanged">
      Сохранить
    </action-button>
  </form>

</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Action, Getter } from 'vuex-class';
import { cloneDeep, isEqual } from 'lodash';

import { User, ColorSchema, UserSettings } from '@/api/models/community';
import { userIsNanny } from '@/api/models/community/helpers';

const colorSchemaOptions = [{
  value: ColorSchema.Modern,
  label: 'Обычная контрастная',
}, {
  value: ColorSchema.Pale,
  label: 'Обычная',
}, {
  value: ColorSchema.Classic,
  label: 'Классическая контрастная',
  description: 'как на старом сайте',
}, {
  value: ColorSchema.ClassicPale,
  label: 'Классическая',
  description: 'как на старом сайте, но бледнее',
}, {
  value: ColorSchema.Night,
  label: 'Ночная',
  description: 'пока в разработке'
}];

const pagingSuggestions = [{
  value: 10,
}, {
  value: 30,
}, {
  value: 50,
}, {
  value: 100,
  description: 'не рекомендуется'
}];

@Component({})
export default class ProfileSettings extends Vue {
  private pagingSuggestions: any = pagingSuggestions;
  private colorSchemaOptions: any = colorSchemaOptions;

  private saving = false;
  private savedSettings: UserSettings | null = null;

  @Getter('community/editableUser')
  private user!: User | null;

  @Action('community/updateSettings')
  private updateSettings: any;

  @Action('ui/updateTheme')
  private updateTheme: any;

  @Watch('user.settings.colorSchema')
  private onColorSchemaUpdated(theme: ColorSchema) {
    this.updateTheme( {theme });
  }

  @Watch('user')
  private onEditableUserChanged(): void {
    this.updateSavedSettings();
  }

  private mounted(): void {
    this.updateSavedSettings();
  }

  private get unchanged(): boolean {
    return this.savedSettings === null || this.user === null ||
      isEqual(this.savedSettings, this.user!.settings);
  }

  private get userIsNanny(): boolean {
    return userIsNanny(this.user);
  }

  private async save(): Promise<void> {
    this.saving = true;
    await this.updateSettings({ router: this.$router });
    this.updateSavedSettings();
    this.saving = false;
  }

  private updateSavedSettings(): void {
    this.savedSettings = cloneDeep(this.user!.settings) as UserSettings;
  }
}
</script>

<style scoped lang="stylus">
.settings-block
  margin $medium 0

.settings-per-page
  margin $small

.settings-title
  minorTitle()
</style>
