<template>
  <div>

    <action-button @click="$modal.show('create-poll')">
      <icon :font="IconType.Add" /> Новый опрос
    </action-button>

    <lightbox name="create-poll">
      <template slot="title">Новый опрос</template>

      <form @submit.prevent="create">
        <div class="form-field">
          <input v-model="poll.title" id="poll-title" placeholder="Введите вопрос" />
        </div>
        <div>Варианты</div>
        <draggable :list="poll.options" handle=".handle">
          <div v-for="(option, index) in poll.options" :key="index" class="option-container">
            <div class="option-actions">
              <a class="handle"><icon :font="IconType.Reorder" /></a>
              <a @click="removeOption(index)"><icon :font="IconType.Close" /></a>
            </div>
            <div class="form-field">
              <span :for="`option-value-${index}`" class="option-counter">{{ index + 1 }}.</span>
              <input :id="`option-value-${index}`" v-model="option.text" placeholder="Введите вариант" />
            </div>
          </div>
        </draggable>
        <a @click="addOption">
          <icon :font="IconType.Add" /> добавить вариант
        </a>
        <div class="form-field">
          <label for="poll-ends">Окончание</label>
          <input v-model="poll.ends" id="poll-ends" type="datetime-local" />
        </div>
      </form>

      <template slot="controls">
        <action-button @click="create" :loading="creating" :disabled="formEmpty">Создать опрос</action-button>
      </template>
    </lightbox>

  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import IconType from '@/components/iconType';
import { Poll, PollOption } from '@/api/models/community';
import moment from 'moment';
import { Action } from 'vuex-class';
import draggable from 'vuedraggable';

@Component({
  components: {
    draggable,
  }
})
export default class CreatePoll extends Vue {
  private IconType: typeof IconType = IconType;
  private creating = false;

  private poll: Poll = {
    title: '',
    options: [],
    ends: moment().add(2, 'days').format('YYYY-MM-DDThh:mm'), // формат предусмотрен нативным datepicker'ом
  } as Poll;

  private get formEmpty(): boolean {
    return this.poll.title.length === 0 || this.poll.options.length === 0;
  }

  @Action('community/createPoll')
  private createPoll: any;

  private async create(): Promise<void> {
    this.creating = true;
    await this.createPoll({ router: this.$router, poll: this.poll });
    this.creating = false;
    this.$modal.hide('create-poll');
  }

  private addOption() {
    this.poll.options.push({ text: '' } as PollOption);
  }

  private removeOption(index: number) {
    this.poll.options.splice(index, 1);
  }
}
</script>

<style lang="stylus">
.option-container
  position relative
  border-radius $borderRadius

.option-actions
  position absolute
  top $small
  right $small

.option-counter
  margin-right $small
</style>