DM.Lightbox = Base.extend({
    constructor: function (options, view) {
        this._view = view || new DM.Lightbox.View(options);
        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("opened", this.open, this);
        this._view.on("closed", this.close, this);
    },

    open: function () {
        DM.LightboxStack.push(this);
        this.trigger("opened");
    },
    close: function () {
        this.trigger("closed");
    },

    show: function() {
        this._view.show();
    },
    hide: function() {
        this._view.hide();
    },

    getElement: function () { // obsolete
        return this._view.getElement();
    },
    $: function() {
        return this.getElement();
    },
    hasForm: function() {
        return this._view.hasForm();
    },
    remove: function () {
        this._view.remove();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._lightbox = options.lightbox;
            this._openLink = options.openLink;
            this._closeLink = this._lightbox.find(".lightbox-closeButton");
            
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;

            if (this._openLink !== undefined) {
                this._openLink.on("click", function() {
                    _this.trigger("opened");
                });
            }
            this._lightbox.on("click", ".lightbox-close", function() {
                _this.trigger("closed");
            });
        },
        
        show: function() {
            this._lightbox.show();
        },
        hide: function() {
            this._lightbox.hide();
        },

        getElement: function () {
            return this._lightbox;
        },
        hasForm: function() {
            return this._lightbox.find("form").length > 0;
        },
        remove: function () {
            this._lightbox.remove();
        }
    }),
    create: function (data, options) {
        options = options || {};

        options.lightbox = $(data).appendTo("#LightboxContainer");
        return new DM.Lightbox(options);
    }
});