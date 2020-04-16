<template>
  <div class="header-block">
    <div class="title">{{title}}</div>
    <div v-if="subtitle" class="subtitle">{{subtitle}}</div>
  </div>
</template>

<script lang="ts">
    import {Component, Prop, Vue} from 'vue-property-decorator';

    @Component
    export default class HeaderDefault extends Vue {

        @Prop()
        private title!: string;

        @Prop()
        private subtitle?: string;
    }
</script>

<style scoped lang="stylus">
  .header-block
    display flex
    flex-direction column
    align-items center
    justify-content center
    height 100%

    .title
      font-size $titleFontSize
      theme(color, $text)

    .subtitle
      font-size $secondaryFontSize
      theme(color, $secondaryText)
</style>
