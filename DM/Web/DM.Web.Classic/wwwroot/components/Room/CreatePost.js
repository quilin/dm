DM.CreatePostsListEntityControl = DM.CreatePagedListEntityControl.extend({
    constructor: function (options, view, proxy) {
        this._proxy = proxy || new DM.CreatePostsListEntityControl.Proxy();
        this.base(options, view || new DM.CreatePostsListEntityControl.View(options));
    },
    __attachEventListeners: function () {
        this._view.on("showPreviewRequest", this._proxy.showPreview, this._proxy);
        this._proxy.on("showPreviewRequestBegin", this._view.showPreviewRequestBegin, this._view);
        this._proxy.on("showPreviewRequestComplete", this._view.showPreviewRequestComplete, this._view);
        this._proxy.on("showPreviewRequestSuccess", this._view.showPreviewRequestSuccess, this._view);
        this.base();
    }
}, {
    View: DM.CreatePagedListEntityControl.View.extend({
        constructor: function() {
            var $dropdownBind = $("#CreatePostNotifyUsers");
            if ($dropdownBind.length > 0) {
                this._notifyUserIds = new DM.DropdownMultiSelect({
                    bindItem: $dropdownBind,
                    defaultLabel: "Жду сообщения от..."
                });
            }

            this._diceControl = new DM.DiceControl({});

            this._previewLink = $("#CreatePostPreviewLink").hide();
            this._previewLinkLoader = DM.Loader.create(this._previewLink);

            this.base();

            var _this = this;
            this._form.interfere(function(data) {
                return data + "&" + _this._diceControl.getDiceIdsEncoded();
            });
        },
        __attachEventListeners: function() {
            if (this._notifyUserIds) {
                this._notifyUserIds.on("valueChanged", this._form.resolveEnabled, this._form);
            }
            this._diceControl.on("diceChanged", this._form.resolveEnabled, this._form);
            this._form
                .on("requestSuccess", this._diceControl.clear, this._diceControl)
                .on("toggledEnable", this._togglePreviewLink, this);

            var _this = this;
            this._previewLink.on("click.showPreviewRequest", function(evt) {
                evt.preventDefault();
                _this.trigger("showPreviewRequest", this.href, _this._form.serialize());
            });

            this.base();
        },
        _togglePreviewLink: function(enabled) {
            this._previewLink.toggle(enabled && this._form.isValid());
        },
        showPreviewRequestBegin: function() {
            this._previewLinkLoader.show();
        },
        showPreviewRequestComplete: function() {
            this._previewLinkLoader.hide();
        },
        showPreviewRequestSuccess: function (data) {
            if (this._previewLightbox) {
                this._previewLightbox.remove();
            }
            this._previewLightbox = DM.Lightbox.create(data);
            this._previewLightbox.open();
        }
    }),
    Proxy: Base.extend({
        showPreview: function(url, data) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("showPreviewRequestBegin"),
                complete: this.handle("showPreviewRequestComplete"),
                success: this.handle("showPreviewRequestSuccess")
            });
        }
    })
});