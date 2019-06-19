DM.Placeholder = Base.extend({
        constructor: function(options, view) {
            this._view = view || new DM.Placeholder.View(options);

            this.__attachEventListeners();
            this.__init();
        },
        __attachEventListeners: function() {
            this._view.on("valueChanged", this._resolveValue, this);
        },
        __init: function () {
            this._resolveValue(this._view.getValue());
        },
        _resolveValue: function (value) {
            if (value.length === 0) {
                this._view.show();
            } else {
                this._view.hide();
            }
        }
    }, {
        View: Base.extend({
            constructor: function(options) {
                this._node = options.input;
                this._placeholder = this._node.closest(".placeholder-input-wrapper").find(".placeholder");

                this._node.data("initiated", true);

                this.__attachEventListeners();
            },
            __attachEventListeners: function() {
                var _this = this;
                this._node.on("input.valueChanged", function() {
                    _this.trigger("valueChanged", this.value);
                });
            },
            show: function() {
                this._placeholder.show();
            },
            hide: function() {
                this._placeholder.hide();
            },
            getValue: function () {
                return this._node.val();
            }
        }),
        create: function(input) {
            return new DM.Placeholder({
                input: input
            });
        }
    });