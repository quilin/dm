var Tooltip = PopupBase.extend({
    constructor: function(options, view) {
        this.base(options, view || new Tooltip.View(options));
    }
}, {
    View: PopupBase.View.extend({
        constructor: function(options) {
            this.base.options();
        }
    })
});