DM.FormLightboxControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.FormLightboxControl.View(options);
        this._proxy = proxy || new DM.FormLightboxControl.Proxy(options);

        this._loaded = false;

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("lightboxRequest", this._resolveRequest, this);
        this._proxy.on("lightboxRequestBegin", this._view.lightboxRequestBegin, this._view);
        this._proxy.on("lightboxRequestComplete", this._view.lightboxRequestComplete, this._view);
        this._proxy.on("lightboxRequestSuccess", this._resolveRequestResult, this);

        this._view.on("requestSuccess", this.handle("requestSuccess"), this);
    },
    _resolveRequest: function(url) {
        if (!this._loaded) {
            this._proxy.getLightbox(url);
        }
    },
    _resolveRequestResult: function(data) {
        this._loaded = true;
        this._view.initLightbox(data);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._link = options.link;
            this._loader = DM.Loader.create(this._link);

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._link.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("lightboxRequest", this.href);
            });
        },
        lightboxRequestBegin: function() {
            this._loader.show();
        },
        lightboxRequestComplete: function() {
            this._loader.hide();
        },

        initLightbox: function(data) {
            this._lightbox = DM.Lightbox.create(data, {
                openLink: this._link
            });

            this._form = new DM.FormControl(this._lightbox.$().find("form"), {
                validate: true,
                initPlaceholder: true
            });
            this._form.on("requestSuccess", this._resolveResult, this);

            this._lightbox.open();
        },
        _resolveResult: function (data) {
            this._lightbox.close();
            this._form.reset();

            this.trigger("requestSuccess", data);
        }
    }),
    Proxy: Base.extend({
        getLightbox: function(url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("lightboxRequestBegin"),
                complete: this.handle("lightboxRequestComplete"),
                success: this.handle("lightboxRequestSuccess")
            });
        }
    })
});