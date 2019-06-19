DM.LoginControl = DM.AuthenticationActionControl.extend({
    constructor: function (data, view, proxy, passwordRestorationControlFactory) {
        this.base(data, view || new DM.LoginControl.View(data, "LoginLink"));
        this._proxy = proxy || new DM.LoginControl.Proxy();
        this._passwordRestorationControlFactory = passwordRestorationControlFactory || new DM.LoginControl.PasswordRestorationControlFactory();
        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("restorePasswordRequest", this._proxy.getRestorePasswordForm, this._proxy);
        this._proxy.on("restorePasswordRequestBegin", this._view.restorePasswordRequestBegin, this._view);
        this._proxy.on("restorePasswordRequestComplete", this._view.restorePasswordRequestComplete, this._view);
        this._proxy.on("restorePasswordRequestSuccess", this._restorePasswordRequestSuccess, this);
    },
    _restorePasswordRequestSuccess: function(data) {
        if (!this._passwordRestorationControl) {
            this._passwordRestorationControl = this._passwordRestorationControlFactory.create(data);
        }
    }
}, {
    View: DM.AuthenticationActionControl.View.extend({
        constructor: function (data, linkId) {
            this.base(data, linkId);

            this._restorePasswordLink = $("#RestorePasswordLink");
            this._restorePasswordLoader = DM.Loader.create(this._restorePasswordLink);
            
            $("#RedirectAfterLogin").val(document.location.href);
        },
        attachEventListeners: function () {
            this.base();

            var _this = this;
            this._restorePasswordLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("restorePasswordRequest", this.href);
            });
        },
        restorePasswordRequestBegin: function() {
            this._restorePasswordLoader.show();
        },
        restorePasswordRequestComplete: function() {
            this._restorePasswordLoader.hide();
        }
    }),
    Proxy: Base.extend({
        getRestorePasswordForm: function(url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("restorePasswordRequestBegin"),
                complete: this.handle("restorePasswordRequestComplete"),
                success: this.handle("restorePasswordRequestSuccess")
            });
        }
    }),
    PasswordRestorationControlFactory: Base.extend({
        create: function(data) {
            return new DM.PasswordRestorationControl(data);
        }
    })
});