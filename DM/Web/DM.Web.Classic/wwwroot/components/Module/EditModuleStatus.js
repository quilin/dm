DM.EditModuleStatusControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.EditModuleStatusControl.View(options);
        this._proxy = proxy || new DM.EditModuleStatusControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("getFormRequest", this._proxy.getForm, this._proxy);
        this._proxy.on("getFormRequestBegin", this._view.getFormRequestBegin, this._view);
        this._proxy.on("getFormRequestComplete", this._view.getFormRequestComplete, this._view);
        this._proxy.on("getFormRequestSuccess", this._view.getFormRequestSuccess, this._view);

        this._view.on("takeOnPremoderationRequest", this._proxy.takeOnPremoderation, this._proxy);
        this._proxy.on("takeOnPremoderationRequestBegin", this._view.takeOnPremoderationRequestBegin, this._view);
        this._proxy.on("takeOnPremoderationRequestComplete", this._view.takeOnPremoderationRequestComplete, this._view);
        this._proxy.on("takeOnPremoderationRequestSuccess", this._view.takeOnPremoderationRequestSuccess, this._view);
    }
}, {
    View: Base.extend({
        constructor: function () {
            this._getFormLink = $("#GetEditModuleStatusLink");
            this._getFormLoader = DM.Loader.create(this._getFormLink);

            this._takeOnPremoderationLink = $("#PremoderateLink");
            this._takeOnPremoderationLinkLoader = DM.Loader.create(this._takeOnPremoderationLink);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._getFormLink.on("click", function(evt) {
                evt.preventDefault();
                _this.trigger("getFormRequest", this.href);
            });
            this._takeOnPremoderationLink.on("click", function(evt) {
                evt.preventDefault();
                _this.trigger("takeOnPremoderationRequest", this.href);
            });
        },
        getFormRequestBegin: function () {
            this._getFormLoader.show();
        },
        getFormRequestComplete: function () {
            this._getFormLoader.hide();
        },
        getFormRequestSuccess: function (data) {
            if (this._editStatusLightbox !== undefined) {
                this._editStatusLightbox.remove();
            }

            this._editStatusLightbox = DM.Lightbox.create(data);
            this._editStatusLightbox.open();
            this._editStatusForm = new DM.FormControl("#EditModuleStatusForm");
            this._editStatusForm.on("requestSuccess", this.changeStatusRequestSuccess);
        },
        changeStatusRequestSuccess: function (data) {
            document.location.href = data;
        },

        takeOnPremoderationRequestBegin: function() {
            this._takeOnPremoderationLinkLoader.show();
        },
        takeOnPremoderationRequestComplete: function() {
            this._takeOnPremoderationLinkLoader.hide();
        },
        takeOnPremoderationRequestSuccess: function(data) {
            document.location.href = data;
        }
    }),
    Proxy: Base.extend({
        getForm: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getFormRequestBegin"),
                complete: this.handle("getFormRequestComplete"),
                success: this.handle("getFormRequestSuccess")
            });
        },
        takeOnPremoderation: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("takeOnPremoderationRequestBegin"),
                complete: this.handle("takeOnPremoderationRequestComplete"),
                success: this.handle("takeOnPremoderationRequestSuccess")
            });
        }
    })
});
var editModuleStatusControl = new DM.EditModuleStatusControl();