DM.AuthenticationActionControl = Base.extend({
    constructor: function (data, view) {
        this._view = view || new DM.AuthenticationActionControl.View(data);
        this._view.attachEventListeners();
    }
}, {
    View: Base.extend({
        constructor: function (data, linkId) {
            this._actionLink = $("#" + linkId);

            this._lightbox = DM.Lightbox.create(data, { openLink: this._actionLink });
            this._lightbox.open();

            this._form = new DM.FormControl(this._lightbox.getElement().find("form"), {
                validate: true,
                keepLoader: this.__keepLoader
            });
        },
        __keepLoader: true,
        attachEventListeners: function () {
            this._form.on("requestSuccess", this.requestSuccess, this);
        },
        requestSuccess: function (redirectUrl) {
            window.location.href = redirectUrl;
        }
    })
});