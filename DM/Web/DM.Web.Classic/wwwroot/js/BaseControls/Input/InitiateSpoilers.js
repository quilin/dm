$(document).on("click.toggleSpoiler", ".spoiler-head", function (evt) {
    evt.preventDefault();
    var $this = $(this);
    $this.next().toggle();
    $this.swapText();
});