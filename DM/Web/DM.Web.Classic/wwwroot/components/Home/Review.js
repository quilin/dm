DM.ReviewControl = Base.extend({
    constructor: function (options, view, proxy) {
        this._reviewId = options.reviewId;

        this._view = view || new DM.ReviewControl.View(options);
        this._proxy = proxy || new DM.ReviewControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("approveRequest", this._proxy.approve, this._proxy);
        this._proxy.on("approveRequestBegin", this._view.approveRequestBegin, this._view);
        this._proxy.on("approveRequestComplete", this._view.approveRequestComplete, this._view);
        this._proxy.on("approveRequestSuccess", this._approveRequestSuccess, this);

        this._view.on("removeRequest", this._proxy.remove, this._proxy);
        this._proxy.on("removeRequestBegin", this._view.removeRequestBegin, this._view);
        this._proxy.on("removeRequestComplete", this._view.removeRequestComplete, this._view);
        this._proxy.on("removeRequestSuccess", this._removeRequestSuccess, this);
    },
    _approveRequestSuccess: function(data) {
        this._view.approveRequestSuccess(data);
        this.trigger("approved", this._reviewId);
    },
    _removeRequestSuccess: function() {
        this._view.removeRequestSuccess();
        this.trigger("removed", this._reviewId);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._reviewId = options.reviewId;

            this._container = $("#ReviewContainer_" + this._reviewId);
            this._approveLink = $("#ApproveReviewLink_" + this._reviewId);
            this._approveLinkLoader = new DM.Loader({ bindItem: this._approveLink });
            this._removeLink = $("#RemoveReviewLink_" + this._reviewId);
            this._removeLinkLoader = new DM.Loader({ bindItem: this._removeLink });

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._approveLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("approveRequest", this.href);
            });
            this._removeLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("removeRequest", this.href);
            });
        },
        approveRequestBegin: function() {
            this._approveLinkLoader.show();
        },
        approveRequestComplete: function() {
            this._approveLinkLoader.hide();
        },
        approveRequestSuccess: function(data) {
            this._container.replaceWith(data);
        },
        removeRequestBegin: function() {
            this._removeLinkLoader.show();
        },
        removeRequestComplete: function() {
            this._removeLinkLoader.hide();
        },
        removeRequestSuccess: function() {
            this._container.remove();
        }
    }),
    Proxy: Base.extend({
        approve: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("approveRequestBegin"),
                complete: this.handle("approveRequestComplete"),
                success: this.handle("approveRequestSuccess")
            });
        },
        remove: function(url) {
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
});