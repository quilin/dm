import { Component, Watch, Vue } from 'vue-property-decorator';

@Component({})
export default class PopupBase extends Vue {
  public $refs!: {
    container: HTMLElement;
  }

  protected popupShown: boolean = false;

  protected top: string = '0';
  protected left: string = '0';

  @Watch('popupShown')
  private onToggle(): void {
    this.calculatePosition();
  }

  private mounted(): void {
    document.addEventListener('click', this.handleClick);
    window.addEventListener('resize', this.calculatePosition);

    const { scroll } = this.$root.$children[0].$refs;
    const scrollElement = scroll as HTMLElement;
    scrollElement.addEventListener('scroll', this.calculatePosition);
  }

  private beforeDestroy(): void {
    document.removeEventListener('click', this.handleClick);
    window.removeEventListener('resize', this.calculatePosition);

    const { scroll } = this.$root.$children[0].$refs;
    const scrollElement = scroll as HTMLElement;
    scrollElement.removeEventListener('scroll', this.calculatePosition);
  }

  private calculatePosition(): void {
    if (!this.popupShown) return;

    const { container } = this.$refs;
    const { top, left } = container.getBoundingClientRect();

    this.top = `${top + container.offsetHeight}px`;
    this.left = `${left}px`;
  }

  private handleClick(event: MouseEvent): void {
    const { target } = event;
    const { $el } = this;
    if (!$el.contains(target as Node)) {
      this.popupShown = false;
    }
  }
}