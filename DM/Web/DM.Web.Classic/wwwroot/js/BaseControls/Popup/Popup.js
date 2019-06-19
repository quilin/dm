DM.Popup = DM.PopupBase.extend({
        constructor: function(options, view) {
            this.base(options, view || new DM.Popup.View(options));

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            this.base();

            this._view.on("fadedIn", function () {
                this.displayed = true;
                this.trigger("show");
            }, this);
            this._view.on("fadedOut", function() {
                this.hide();
            }, this);
        },
        fadeIn: function() {
            if (!this.displayed) {
                this._view.fadeIn();
            }
        },
        fadeOut: function() {
            if (this.displayed) {
                this._view.fadeOut();
            }
        },
        setPosition: function(position) {
            this._view.setPosition(position);
            this.renderPosition();
        },
        setSize: function(size) {
            this._view.setSize(size);
        },
        animateToPosition: function(position) {
            if (this.displayed) {
                this._view.animateToPosition(position);
            } else {
                this.setPosition(position);
            }
        },
        bindTo: function(element) {
            this._view.bindTo(element);
            this.renderPosition();
        }
    }, {
        View: DM.PopupBase.View.extend({
            constructor: function(options) {
                this.base(options);
                
                this.__init();
            },
            fadeIn: function() {
                this.renderPosition();
                var _this = this;
                this._popupItem.fadeIn(this._animationTime, function() { _this.trigger("fadedIn"); });
            },
            fadeOut: function() {
                var _this = this;
                this._popupItem.fadeOut(this._animationTime, function() { _this.trigger("fadedOut", null, true); });
            },
            bindTo: function(element) {
                this._bindItem = element;
            },
            setPosition: function(position) {
                this._position = position;
            },
            animateToPosition: function(position) {
                this._position = position;
                this._popupItem.animate(this._countPosition(), this._animationTime);
            },
            setSize: function(size) {
                this._popupItem.css(size);
            }
        })
    });