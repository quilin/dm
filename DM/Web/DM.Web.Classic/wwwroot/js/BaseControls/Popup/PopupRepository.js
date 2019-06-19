DM.PopupRepository = {
    init: function(options, view) {
        this._view = view || new DM.PopupRepository.View(options);

        this._popups = { };
        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("closeInitiated", function () {
            for (var popupId in this._popups) {
                var popup = this._popups[popupId];
                if (!popup.fixed && popup.displayed) {
                    popup.hide();
                }
            }
        }, this);
    },
    register: function(popup) {
        var popupId = DM.GuidFactory.create();
        this._popups[popupId] = popup;
        return popupId;
    },
    pull: function(popupId) {
        delete this._popups[popupId];
    },
    View: Base.extend({
        constructor: function() {
            this._contentWrapper = $("#ContentWrapper");
            this._footerWrapper = $("#FooterWrapper");
            this._lightboxContainer = $("#LightboxContainer");
            this._mainWrapper = $("#MainWrapper");
            this._contentContainer = $("#ContentContainer");

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._contentWrapper.on("click.initClose", function () {
                _this.trigger("closeInitiated");
            });
            this._footerWrapper.on("click.initClose", function () {
                _this.trigger("closeInitiated");
            });
            this._lightboxContainer.on("click.initClose", function () {
                _this.trigger("closeInitiated");
            });
            this._mainWrapper.on("click.tryClose", function (evt) {
                if (evt.originalEvent.target === _this._mainWrapper[0]) {
                    _this.trigger("closeInitiated");
                }
            });
            this._contentContainer.on("click.tryClose", function (evt) {
                if (evt.originalEvent.target === _this._contentContainer[0]) {
                    _this.trigger("closeInitiated");
                }
            });
            $(document).on("keydown.initClose", function (evt) {
                var btn = evt.keyCode || evt.which;
                if (btn == 27) {
                    _this.trigger("closeInitiated");
                }
            });
        }
    })
};

DM.PopupRepository.init();