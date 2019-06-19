DM.EntityWarningControl = Base.extend({
    constructor: function (options, view, proxy) {
        this._view = view || new DM.EntityWarningControl.View(options);
        this._proxy = proxy || new DM.EntityWarningControl.Proxy(options);

        this._warningId = options.warningId;

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("removeRequest", this._proxy.remove, this._proxy);
        this._proxy.on("removeRequestBegin", this._view.removeRequestBegin, this._view);
        this._proxy.on("removeRequestComplete", this._view.removeRequestComplete, this._view);
        this._proxy.on("removeRequestSuccess", this.handle("removed"), this);
    },
    cancelEdit: function() {
        
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._warningId = options.warningId;

            this._block = $("Warning_" + this._warningId);

            this._content = $("WarningContent_" + this._warningId);

            this._editLink = $("#EditWarningLink_" + this._warningId);
            this._removeLink = $("#RemoveWarningLink_" + this._warningId);

            this._editLinkLoader = DM.Loader.create(this._editLink);
            this._removeLinkLoader = DM.Loader.create(this._removeLink);

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._removeLink.on("click.request", function (evt) {
                evt.preventDefault();
                var href = this.href;
                DM.Confirm().done(function () {
                    _this.trigger("removeRequest", href);
                });
            });

            this._editLink.on("click.request", function (evt) {
                evt.preventDefault();
                var href = this.href;
                console.log("Not implemented yet");
            });
        },

        removeRequestBegin: function() {
            this._removeLinkLoader.show();
        },
        removeRequestComplete: function () {
            this._removeLinkLoader.hide();
        },


    }),
    Proxy: Base.extend({
        constructor: function(options) {
            this._warningId = options.warningId;

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            
        },
        remove: function (url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("removeRequestBegin"),
                complete: this.handle("removeRequestComplete"),
                success: this.handle("removeRequestSuccess")
            });
        }
    })
   })