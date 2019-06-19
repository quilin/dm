(function($) {
    $(document).on("keydown.initiatePlaceholder", ".js-placeholder", function() {
        var $this = $(this);
        if (!$this.data("initiated")) {
            $this.placeholder();
        }
    });

    $(".placeholder-input-wrapper").find("textarea, input").trigger("keydown");
})(jQuery);