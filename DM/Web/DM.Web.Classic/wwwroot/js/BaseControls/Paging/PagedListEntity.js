DM.PagedListEntityControl = Base.extend({
    constructor: function (options, view, proxy, editEntityControlFactory, warningsList) {
        this._entityId = options.entityId;

        this._view = view || new DM.PagedListEntityControl.View(options);
        this._proxy = proxy || new DM.PagedListEntityControl.Proxy(options);
        this._editEntityControlFactory = editEntityControlFactory || new DM.PagedListEntityControl.EditEntityControlFactory(options);

        if (options.withWarnings) {
            this._warningsList = warningsList || new DM.EntityWarningsListControl(options);
        }

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("removeRequest", this._proxy.remove, this._proxy);
        this._proxy.on("removeRequestBegin", this._view.removeRequestBegin, this._view);
        this._proxy.on("removeRequestComplete", this._view.removeRequestComplete, this._view);
        this._proxy.on("removeRequestSuccess", this.handle("removed"), this);

        this._view.on("getEditFormRequest", this._proxy.getEditForm, this._proxy);
        this._proxy.on("getEditFormRequestBegin", this._view.getEditFormRequestBegin, this._view);
        this._proxy.on("getEditFormRequestComplete", this._view.getEditFormRequestComplete, this._view);
        this._proxy.on("getEditFormRequestSuccess", this._getEditFormRequestSuccess, this);
        this._view.on("toggleLikeRequest", this._proxy.toggleLike, this._proxy);
        this._proxy.on("toggleLikeRequestBegin", this._view.toggleLikeRequestBegin, this._view);
        this._proxy.on("toggleLikeRequestComplete", this._view.toggleLikeRequestComplete, this._view);
        this._proxy.on("toggleLikeRequestSuccess", this._view.toggleLikeRequestSuccess, this._view);

        this._view.on("editInitiated", this._handleEditInitiated, this);
    },
    _handleEditInitiated: function () {
        if (this._warningsList) {
            this._warningsList.cancelWarnings();
        }
        this.trigger("editInitiated", this._entityId);
    },
    _getEditFormRequestSuccess: function (data) {
        this._view.getEditFormRequestSuccess(data);
        this.__initEntityEditControl();
        this._handleEditInitiated();
    },
    __initEntityEditControl: function () {
        this._editEntityControl = this._editEntityControlFactory.create();
        this._editEntityControl
            .on("edited", this._edit, this)
            .on("canceled", this._view.cancelEdit, this._view);
    },
    _edit: function (data) {
        if (this._warningsList) {
            this._warningsList.cancelWarnings();
        }
        this._view.edit(data);
        this.trigger("edited", this._entityId);
    },
    cancelEdit: function() {
        this._view.cancelEdit();
        if (this._warningsList) {
            this._warningsList.cancelWarnings();
        }
    },
    highlight: function() {
        this._view.highlight();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._entityId = options.entityId;

            this._block = $("#Entity_" + this._entityId);
            this._content = $("#EntityContent_" + this._entityId);
            this._editLink = $("#EditEntityLink_" + this._entityId);
            this._removeLink = $("#RemoveEntityLink_" + this._entityId);

            this._likeLink = $("#ToggleEntityLikeLink_" + this._entityId);
            this._likesCounter = $("#EntityLikesCounter_" + this._entityId);

            this._editLinkLoader = DM.Loader.create(this._editLink);
            this._removeLinkLoader = DM.Loader.create(this._removeLink);
            this._likeLinkLoader = DM.Loader.create(this._likeLink);

            this._editFormPlaceholder = this._block.find(".comment-edit-form-wrapper");

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._removeLink.on("click.request", function (evt) {
                evt.preventDefault();
                var href = this.href;
                DM.Confirm().done(function() {
                    _this.trigger("removeRequest", href);
                });
            });
            this._editLink.on("click.request", function (evt) {
                evt.preventDefault();
                _this.trigger("getEditFormRequest", this.href);
            });
            this._likeLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("toggleLikeRequest", this.href);
            });
        },
        removeRequestBegin: function () {
            this._removeLinkLoader.show();
        },
        removeRequestComplete: function () {
            this._removeLinkLoader.hide();
        },

        // Likes
        toggleLikeRequestBegin: function() {
            this._likeLinkLoader.show();
        },
        toggleLikeRequestComplete: function() {
            this._likeLinkLoader.hide();
        },
        toggleLikeRequestSuccess: function(data) {
            this._likeLink.toggleClass("comment-likes-active", data.created);
            this._likesCounter.text(data.likesCount > 0 ? data.likesCount : "");
            this._likeLink.parent().toggleClass("comment-likes-wrapper-hasLikes", data.likesCount > 0);
        },

        // Edit form
        getEditFormRequestBegin: function () {
            this._editLinkLoader.show();
        },
        getEditFormRequestComplete: function() {
            this._editLinkLoader.hide();
        },
        getEditFormRequestSuccess: function (data) {
            var _this = this;
            this._editForm = this._editFormPlaceholder.html(data);
            this._editLink
                .off("click.request")
                .on("click.toggleForm", function(evt) {
                    evt.preventDefault();
                    _this.showEditForm();
                    _this.trigger("editInitiated");
                });
            this.showEditForm();
        },
        edit: function(data) {
            this._block.replaceWith(data);
        },
        cancelEdit: function () {
            if (this._editForm) {
                this._editForm.hide();
                this._content.show();
            }
        },
        showEditForm: function () {
            this._editForm.show();
            this._content.hide();
        },
        highlight: function () {
            $("#MainWrapper").animate({
                scrollTop: this._block[0].offsetTop + "px"
            }, 200);
            this._block.addClassTimeout("content-pagedEntity-highlighted", 5000);
        }
        // end Edit form
    }),
    Proxy: Base.extend({
        getEditForm: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getEditFormRequestBegin"),
                complete: this.handle("getEditFormRequestComplete"),
                success: this.handle("getEditFormRequestSuccess")
            });
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
        },
        toggleLike: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("toggleLikeRequestBegin"),
                complete: this.handle("toggleLikeRequestComplete"),
                success: this.handle("toggleLikeRequestSuccess")
            });
        }
    }),
    EditEntityControlFactory: Base.extend({
        constructor: function (options) {
            this._entityId = options.entityId;
        },
        create: function () {
            return new DM.EditPagedListEntityControl({
                entityId: this._entityId
            });
        }
    })
});