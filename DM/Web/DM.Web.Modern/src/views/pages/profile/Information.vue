<template>
  <div>

    <loader v-if="!user" :big="true" />

    <template v-else>
      <a v-if="isOwnUser && !editMode" @click="editMode = true" class="edit-link"><icon :font="IconType.Edit" /></a>
      <div v-if="!editMode" v-html="user.info || 'Пользователь ничего о себе не указал'" />

      <form v-else @submit.prevent="save">
        <text-area v-model="editableUser.info" />
        <div class="edit-controls">
          <action-button type="submit" :loading="saving" :disabled="unchanged">Сохранить</action-button>
          <a @click="editMode = false" class="edit-cancel-link">Отмена</a>
        </div>
      </form>
    </template>

  </div>
</template>

<script lang="ts">
import { Component, Watch, Vue } from 'vue-property-decorator';
import { Getter, Action } from 'vuex-class';

import IconType from '@/components/iconType';
import { User } from '@/api/models/community';

@Component({})
export default class ProfileInformation extends Vue {
  private IconType: typeof IconType = IconType;

  private editMode = false;
  private saving = false;
  private savedInfo: string | null = null;

  @Action('community/updateInformation')
  private updateInformation: any;

  @Getter('user')
  private currentUser!: User | null;

  @Getter('community/selectedUser')
  private user!: User | null;

  @Getter('community/editableUser')
  private editableUser!: User | null;

  @Watch('editableUser')
  private onUserChanged(): void {
    this.updateSavedInfo();
  }

  private get unchanged(): boolean {
    return this.editableUser === null || this.editableUser!.info === this.savedInfo;
  }

  private get isOwnUser(): boolean {
    return this.user?.login === this.currentUser?.login;
  }

  private mounted(): void {
    this.updateSavedInfo();
  }

  private async save(): Promise<void> {
    this.saving = true;
    await this.updateInformation({ router: this.$router });
    this.updateSavedInfo();
    this.saving = false;
    this.editMode = false;
  }

  private updateSavedInfo(): void {
    this.savedInfo = this.editableUser!.info;
  }
}
</script>

<style scoped lang="stylus">
.edit-link
  float right

.edit-controls
  margin-top $small

.edit-cancel-link
  display inline-block
  margin-left $medium
</style>
