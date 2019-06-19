DM.AlertControl = Base.extend({
    constructor: function(view) {
        this._view = view || new DM.AlertControl.View();
    },
    init: function (message) {
        this._view.init(message);
    }
}, {
    View: Base.extend({
        constructor: function() {
            this._lightbox = DM.Lightbox.create($("#__Alert__"));
            this._alertText = $("#__Alert__Text");
        },
        init: function(message) {
            this._lightbox.open();
            this._alertText.html(message);
        }
    })
});

DM.Alert = function (message) {
    this._alert = this._alert || new DM.AlertControl();
    this._alert.init(message);
}