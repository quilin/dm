DM.Loader = Base.extend({
    constructor: function (options, view, popupBuilder) {
        this._view = view || new DM.Loader.View(options);
        this._popupBuilder = popupBuilder || new DM.Loader.PopupBuilder(options);
    },
    show: function () {
        if (this._popup === undefined)
            this._popup = this._popupBuilder.build();
        
        this._popup.setSize(this._view.getBindItemSize());
        this._popup.show();
    },
    hide: function () {
        if (this._popup !== undefined)
            this._popup.hide();
    },
    bindTo: function (element) {
        this._view.setBindItem(element);
        if (this._popup === undefined) {
            this._popupBuilder.setBindItem(element);
        } else {
            this._popup.bindTo(element);
        }
    },
    remove: function () {
        if (this._popup !== undefined)
            this._popup.remove();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._bindItem = options.bindItem;
        },
        getBindItemSize: function () {
            return {
                width: this._bindItem.outerWidth(),
                height: this._bindItem.outerHeight()
            };
        },
        setBindItem: function(element) {
            this._bindItem = element;
        }
    }),
    PopupBuilder: Base.extend({
        constructor: function(options) {
            this._bindItem = options.bindItem;
            this._isSticky = options.sticky;
        },
        setBindItem: function (element) {
            this._bindItem = element;
        },
        build: function() {
            return new DM.Loader.LoaderPopup({
                bindItem: this._bindItem,
                popupItem: this._createPopupItem(),
                sticky: this._isSticky,
                fixed: true,
                position: {}
            });
        },
        _createPopupItem: function () {
            if (!this._loaderItem)
                this._loaderItem = $("<div/>", {
                    "class": "loader pu-popup",
                    "title": "Подождите, идет загрузка..."
                });

            return this._loaderItem;
        }
    }),
    LoaderPopup: DM.Popup.extend({
        show: function() {
            if (!this.displayed) {
                this.displayed = true;
                this._view.show();
                this.trigger("show");
            }
        }
    }),
    create: function (bindElement) {
        return new DM.Loader({
            bindItem: bindElement instanceof $ ? bindElement : $(bindElement)
        });
    }
});