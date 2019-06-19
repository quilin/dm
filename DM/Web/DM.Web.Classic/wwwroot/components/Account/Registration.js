DM.RegisterControl = DM.AuthenticationActionControl.extend({
    constructor: function (data, view) {
        this.base(data, view || new DM.RegisterControl.View(data, "RegisterLink"));
    }
}, {
    View: DM.AuthenticationActionControl.View.extend({
        constructor: function (data, linkId) {
            this._successMessageLightbox = DM.Lightbox.create("#RegistrationSuccessMessage");
            this.base(data, linkId);
        },
        requestSuccess: function () {
            DM.LightboxStack.closeAll();
            this._successMessageLightbox.open();
        }
    })
});