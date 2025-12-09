<script setup lang="ts">
import { storeToRefs } from "pinia";
import { useUserStore } from "@/stores";
import { useCommunityStore } from "@/stores/community";
import type { Patch } from "@/api/models";
import type { User } from "@/api/models/community";
import { computed, ref } from "vue";
import useEditMode from "@/composables/useEditMode";
import DmIconButton from "@/components/ui-kit/DmIconButton.vue";
import { IconType } from "@/components/ui-kit/iconType";
import DmForm from "@/components/ui-kit/DmForm.vue";
import DmInput from "@/components/ui-kit/DmInput.vue";
import communityApi from "@/api/requests/communityApi";

const { user: currentUser } = storeToRefs(useUserStore());
const communityStore = useCommunityStore();
const { selectedUser: user } = storeToRefs(communityStore);

const props = defineProps<{
  title: string;
  value: string;
  empty: string;
  updateValue: (value: string) => Patch<User>;
}>();
const editableValue = ref(props.value);

const canEdit = computed(() => currentUser.value!.login === user.value!.login);
const { id, isActive, acquire, release } = useEditMode();

const loading = ref(false);
const saveChanges = async () => {
  if (props.value !== editableValue.value) {
    loading.value = true;
    await communityApi.updateUser(
      user.value!.login,
      props.updateValue(editableValue.value),
    );
    await communityStore.trySelectProfile(user.value!.login);
    loading.value = false;
  }
  release();
};
</script>

<template>
  <dl>
    <secondary-text tag="dt">{{ title }}</secondary-text>
    <dd>
      <div class="profile_stat-data" v-if="!isActive">
        <div class="profile_stat-value">{{ value || empty }}</div>
        <dm-icon-button v-if="canEdit" :icon="IconType.Edit" @click="acquire" />
      </div>
      <dm-form class="profile_stat-edit" v-else @submit="saveChanges">
        <dm-input :id="id" v-model="editableValue" /><dm-icon-button
          type="submit"
          :icon="IconType.Tick"
          :loading="loading"
        /><dm-icon-button :icon="IconType.Close" @click="release" />
      </dm-form>
    </dd>
  </dl>
</template>

<style scoped lang="sass">
@use "@/assets/styles/Variables"

dl
  margin: Variables.$small 0

dd
  padding: 0

.profile_stat-data
  display: flex
  flex-wrap: nowrap
  align-content: space-between
  align-items: baseline

.profile_stat-value
  flex-grow: 1

.profile_stat-edit
  display: flex
  flex-wrap: nowrap
  align-content: space-between
  align-items: baseline

  & > *:first-child
    flex-grow: 1
    min-width: 0
    margin-right: Variables.$minor

  & input
    display: block
    width: 100%
    box-sizing: border-box

  & .icon-button
    margin-left: Variables.$tiny
</style>
