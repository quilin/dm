DM.TimeZoneControl = (function(global, doc, base) {
    var TzControl = base.extend({
        constructor: function(options, view) {
            this._view = view || new TzControl.View(options);
        }
    }, {
        View: Base.extend({
            constructor: function(options) {
                this._tzOffset = new Date().getTimezoneOffset();
                doc.cookie = "__Timezone__Offset__=" + this._tzOffset + "; path=/";
            }
        })
    });
    return new TzControl();
})(window, document, Base);