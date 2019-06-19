DM.PasswordRestorationControl = DM.AuthenticationActionControl.extend({
    constructor: function(data, view) {
        this.base(data, view || new DM.PasswordRestorationControl.View(data, "RestorePasswordLink"));
    }
}, {
    View: DM.AuthenticationActionControl.View.extend({
        constructor: function (data, linkId) {
            this._successMessageLightbox = DM.Lightbox.create("#PasswordRestorationSuccessMessage");
            this.base(data, linkId);
        },
        __keepLoader: false,
        requestSuccess: function() {
            DM.LightboxStack.closeAll();
            this._successMessageLightbox.open();
        }
    })
});