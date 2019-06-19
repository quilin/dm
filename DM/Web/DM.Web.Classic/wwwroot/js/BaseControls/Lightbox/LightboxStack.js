DM.LightboxStack = (function() {
    var Stack = Base.extend({
        constructor: function(view) {
            this._view = view || new Stack.View();

            this._stack = [];

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            this._view.on("allClosed", this.closeAll, this);
            this._view.on("overlayClose", this.overlayClose, this);
        },

        push: function(lightbox) {
            this._hideLightboxes();
            this._stack.push(lightbox);
            lightbox.show();

            var _this = this;
            lightbox.rebind("closed.stack", function () {
                _this._pop(this);
            });

            if (this._stack.length === 1) { // first lightbox should be shown with overlay
                this._view.showOverlay();
            }
        },
        _pop: function() {
            this._stack.pop().hide();
            if (this._stack.length > 0) {
                this._stack[this._stack.length - 1].show();
            } else {
                this._view.hideOverlay();
            }
        },

        _hideLightboxes: function() {
            for (var i = 0; i < this._stack.length; ++i) {
                this._stack[i].hide();
            }
        },
        closeAll: function() {
            while (this._stack.length) {
                this._pop();
            }
        },
        overlayClose: function() {
            if (!this._stack[this._stack.length - 1].hasForm()) {
                this._pop();
            }
        }
    }, {
        View: Base.extend({
            constructor: function() {
                this._overlay = $("#LightboxContainer, #LightboxOverlay");
                this._overlayContainer = $("#LightboxContainer");
                this.__attachEventListeners();
            },
            __attachEventListeners: function() {
                var _this = this;
                $(document).on("keydown", function (evt) {
                    var btn = evt.keyCode || evt.which;
                    if (btn === 27) {
                        _this.trigger("allClosed");
                    }
                });
                this._overlayContainer.on("click.tryClose", function(evt) {
                    if (evt.currentTarget === evt.srcElement) {
                        _this.trigger("overlayClose");
                    }
                });
            },

            showOverlay: function() {
                this._overlay.fadeIn(200);
            },
            hideOverlay: function() {
                this._overlay.fadeOut(200);
            }
        })
    });

    return new Stack();
})();