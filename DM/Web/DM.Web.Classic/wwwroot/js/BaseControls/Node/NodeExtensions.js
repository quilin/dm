(function($) {
    $.fn.mapAsArray = function () {
        return this.map(function(index, item) {
            return $(item);
        });
    };
    $.fn.setInputValue = function(value) {
        this.val(value);
        this.trigger("keydown");
    }
})(jQuery);