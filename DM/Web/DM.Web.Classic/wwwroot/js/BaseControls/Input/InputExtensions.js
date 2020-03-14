(function($) {
    $.fn.resetForm = function () {
        this[0].reset();
        this.find("input[type='text'], input[type='password'], input[type='hidden'], textarea")
            .trigger("input.formReset")
            .trigger("input.valueChanged");
        this.find("input[type='hidden']").trigger("input.formReset");
    };
    $.fn.resetInput = function () {
        return this.each(function() {
            this.value = this.defaultValue;
            $(this).trigger("input.formReset");
        });
    };
    $.fn.swapText = function() {
        var textToSwap = this.data("swaptext");
        this.data("swaptext", this.text());
        this.text(textToSwap);
    };
    $.fn.swapValue = function() {
        var textToSwap = this.data("swaptext");
        this.data("swaptext", this.val());
        this.val(textToSwap);
    };
    $.fn.slideToggleTimeout = function(animationTime, delay) {
        var element = this;
        element.slideDown(animationTime || 200);
        setTimeout(function() {
            element.slideUp(animationTime || 200);
        }, delay || 5000);
    };
    $.fn.addClassTimeout = function(className, delay) {
        var element = this;
        element.addClass(className);
        setTimeout(function() {
            element.removeClass(className);
        }, delay);
    };
})(jQuery);