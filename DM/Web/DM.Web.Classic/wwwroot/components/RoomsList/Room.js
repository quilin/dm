DM.RoomControl = Base.extend({
    constructor: function (options, view, proxy) {
        this._roomId = options.roomId;

        this._view = view || new DM.RoomControl.View(options);
        this._proxy = proxy || new DM.RoomControl.Proxy();

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("getUpdateFormRequest", function (url) {
            if (this._editRoomControl === undefined)
                this._proxy.getUpdateForm(url);
        }, this);
        this._proxy.on("getUpdateFormRequestBegin", this._view.getUpdateFormBegin, this._view);
        this._proxy.on("getUpdateFormRequestComplete", this._view.getUpdateFormComplete, this._view);
        this._proxy.on("getUpdateFormRequestSuccess", function (data) {
            this._view.getUpdateFormSuccess(data);
            this._initEditRoomControl();
        }, this);

        this._view.on("removeRoomRequest", this._proxy.removeRoom, this._proxy);
        this._proxy.on("removeRoomRequestSuccess", function () {
            this._view.remove();
            this.trigger("removed", this._roomId);
        }, this);
    },
    _initEditRoomControl: function () {
        this._editRoomControl = new DM.EditRoomControl({
            roomId: this._roomId
        });

        this._editRoomControl.on("cancel", this._view.closeEditLightbox, this._view);
        this._editRoomControl.on("roomUpdated", function(title) {
            this.updateTitle(title);
            this.closeEditLightbox();
        }, this._view);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._roomId = options.roomId;

            this._roomContainer = $("#RoomContainer_" + this._roomId);
            this._updateRoomLink = $("#UpdateRoomLink_" + this._roomId);
            this._updateRoomLinkLoader = DM.Loader.create(this._updateRoomLink);

            this._removeRoomLink = $("#RemoveRoomLink_" + this._roomId);

            this._roomTitle = $("#RoomTitle_" + this._roomId);
            
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._updateRoomLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("getUpdateFormRequest", this.href);
            });
            this._removeRoomLink.on("click.request", function(evt) {
                evt.preventDefault();
                var href = this.href;
                DM.Confirm().done(function() {
                    _this.trigger("removeRoomRequest", href);
                });
            });
        },
        getUpdateFormBegin: function () {
            this._updateRoomLinkLoader.show();
        },
        getUpdateFormComplete: function () {
            this._updateRoomLinkLoader.hide();
        },
        getUpdateFormSuccess: function (data) {
            this._lightbox = DM.Lightbox.create(data, {
                openLink: this._updateRoomLink
            });
            this._lightbox.open();
        },
        closeEditLightbox: function () {
            this._lightbox.close();
        },
        updateTitle: function (title) {
            this._roomTitle.text(title);
        },
        remove: function() {
            this._roomContainer.remove();
            if (this._lightbox) {
                this._lightbox.remove();
            }
        }
    }),
    Proxy: Base.extend({
        getUpdateForm: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getUpdateFormRequestBegin"),
                complete: this.handle("getUpdateFormRequestComplete"),
                success: this.handle("getUpdateFormRequestSuccess")
            });
        },
        removeRoom: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                success: this.handle("removeRoomRequestSuccess")
            });
        }
    })
});