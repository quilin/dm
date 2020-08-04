import { Component, Watch, Vue } from 'vue-property-decorator';

@Component({})
export default class PopupBase extends Vue {
  public $refs!: {
    container: HTMLElement;
  }

  protected popupShown = false;

  protected top = '0';
  protected left = '0';

  @Watch('popupShown')
  private onToggle() {
    this.calculatePosition();
  }

  private mounted() {
    document.addEventListener('click', this.handleClick);
    window.addEventListener('resize', this.calculatePosition);

    const { scroll } = this.$root.$children[0].$refs;
    const scrollElement = scroll as HTMLElement;
    scrollElement.addEventListener('scroll', this.calculatePosition);
  }

  private beforeDestroy() {
    document.removeEventListener('click', this.handleClick);
    window.removeEventListener('resize', this.calculatePosition);

    const { scroll } = this.$root.$children[0].$refs;
    const scrollElement = scroll as HTMLElement;
    scrollElement.removeEventListener('scroll', this.calculatePosition);
  }

  private calculatePosition() {
    if (!this.popupShown) return;

    const { container } = this.$refs;
    const { top, left } = container.getBoundingClientRect();

    this.top = `${top + container.offsetHeight}px`;
    this.left = `${left}px`;
  }

  private handleClick(event: MouseEvent) {
    const { target } = event;
    const { $el } = this;
    if (!$el.contains(target as Node)) {
      this.popupShown = false;
    }
  }
}
