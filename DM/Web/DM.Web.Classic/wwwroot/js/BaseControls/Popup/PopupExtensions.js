(function($) {
    $.fn.popup = function (options) {
        var id = this.attr("id");
        options = $.extend({
            bindItem: this,
            popupItem: $("#" + id + "_Popup")
        }, options);
        return new Popup(options);
    };
    $.fn.dropdown = function (asDropdown, options) {
        var result = this.map(function () {
            var $bindItem = $(this),
                $next = $bindItem.next(),
                nextIsOptions = $next.hasClass("dds-options");
            var newOptions = $.extend({}, options, {
                bindItem: $bindItem,
                hidden: nextIsOptions ? undefined : $next,
                popupItem: nextIsOptions ? $next : $bindItem.next().next()
            });
            var control = new DM.Dropdown(newOptions);
            return asDropdown ? control : this;
        });

        return asDropdown && result.length === 1 ? result[0] : result;
    };
    $.fn.suggest = function(asSuggest) {
        var result = this.map(function() {
            var $bindItem = $(this),
                $popupItem = $bindItem.next();
            var options = {
                bindItem: $bindItem,
                popupItem: $popupItem
            };
            var control = new DM.Suggest(options);
            return asSuggest ? control : this;
        });

        return asSuggest && result.length === 1 ? result[0] : result;
    }
})(jQuery);