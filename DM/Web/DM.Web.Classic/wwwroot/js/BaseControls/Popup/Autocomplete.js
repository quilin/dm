DM.Autocomplete = DM.Popup.extend({
    constructor: function (options, view, proxy) {
        this._proxy = proxy || new DM.Autocomplete.Proxy(options);
        this.base(options, view || new DM.Autocomplete.View(options));
    },
    __attachEventListeners: function () {
        this.base();
        this._proxy.on("requestBegin", this._view.requestBegin, this._view);
        this._proxy.on("requestComplete", this._view.requestComplete, this._view);
        this._proxy.on("requestError", this._view.requestError, this._view);
        this._proxy.on("requestSuccess", this.requestSuccess, this);

        this._view
            .on("request", this._proxy.request, this._proxy)
            .on("toggle", this._toggle, this)
            .on("optionClick", function (option, evt) {
                this.hide();
                this.trigger("select", option, evt);
                this._view.selectOption(option);
            }, this)
            .on("optionKey", function (option, evt) {
                var btn = evt.keyCode || evt.which;
                if (btn === 27 || btn === 9) { // escape or tab
                    this.hide();
                }
            }, this);
    },
    requestSuccess: function (data) {
        this._view.requestSuccess(data);
        this.show();
    },
    _toggle: function () {
        if (this.displayed) {
            this.hide();
        } else {
            this.show();
        }
    },
    show: function () {
        if (this._view.hasValues()) {
            this.base();
        }
    }
}, {
    View: DM.Popup.View.extend({
        constructor: function (options) {
            this.base(options);

            this._bindItem.attr("autocomplete", "off");
            this._lastValue = null;
            this._loader = DM.Loader.create(this._bindItem);
            this._hidden = options.hidden;

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            this.base();

            var _this = this;
            this._bindItem
                .on("input", $.debounce(function () {
                    this.value = this.value.replace(/^\s+/, "").replace(/\s+$/, "");
                    if (this.value.length > 0 && this.value !== _this._lastValue) {
                        _this._lastValue = this.value;
                        _this._bindItem.removeClass("input-validation-error");
                        _this.trigger("request", this.value);
                    } else {
                        _this.selectEmptyOption();
                    }
                }, 200))
                .on("keydown", function (evt) {
                    switch (evt.keyCode) {
                        case 40:
                            evt.preventDefault();
                            _this._highlightNextOption();
                            break;
                        case 38:
                            evt.preventDefault();
                            _this._highlightPrevOption();
                            break;
                        case 13:
                            evt.preventDefault();
                            _this._selectHighlighted(evt);
                            break;
                    }
                })
                .on("click", function () {
                    _this.trigger("toggle");
                });

            this._popupItem
                .on("mouseenter", ".dds-option-link:not('.dds-option-disabled')", function () {
                    _this._highlightOption($(this).parent());
                })
                .on("click.selectOption", ".dds-option-link:not('.dds-option-disabled')", function (evt) {
                    _this.trigger("optionClick", $(this), evt);
                })
                .on("keydown.resolveOptionKey", ".dds-option-link:not('.dds-option-disabled')", function (evt) {
                    _this.trigger("optionKey", $(this), evt);
                });
        },
        requestBegin: function () {
            this._loader.show();
        },
        requestComplete: function () {
            this._loader.hide();
        },
        requestError: function () {
            this._bindItem.addClass("input-validation-error");
        },
        requestSuccess: function (data) {
            this._popupItem.html("");
            for (var key in data) {
                var option = $("<div/>", {
                    "class": "dds-option"
                }).appendTo(this._popupItem);
                $("<a/>", {
                    "class": "dds-option-link",
                    "data-value": key,
                    "href": "javascript:void(0)",
                    text: data[key]
                }).appendTo(option);
            }
        },
        _highlightOption: function (option) {
            this._popupItem.find(".dds-option").removeClass("dds-option-focused");
            option.addClass("dds-option-focused");
        },
        _highlightNextOption: function () {
            var currentlyHighlightedOption = this._popupItem.find(".dds-option-focused");
            var nextOption = currentlyHighlightedOption.next();
            if (nextOption.length === 0) {
                nextOption = this._popupItem.find(".dds-option").first();
            }

            this._highlightOption(nextOption);
        },
        _highlightPrevOption: function () {
            var currentlyHighlightedOption = this._popupItem.find(".dds-option-focused");
            var prevOption = currentlyHighlightedOption.prev();
            if (prevOption.length === 0) {
                prevOption = this._popupItem.find(".dds-option").last();
            }

            this._highlightOption(prevOption);
        },
        _selectHighlighted: function (evt) {
            var currentlyHighlightedOption = this._popupItem.find(".dds-option-focused");
            if (currentlyHighlightedOption.length !== 0) {
                this.trigger("optionClick", currentlyHighlightedOption.find(".dds-option-link"), evt);
            }
        },

        selectOption: function (option) {
            this._bindItem
                .val(option.text())
                .trigger("input.valueChanged");

            if (this._hidden !== undefined) {
                this._hidden
                    .val(option.data("value"))
                    .trigger("change");
            }
        },
        selectEmptyOption: function () {
            this._bindItem.val("");
            if (this._hidden !== undefined) {
                this._hidden
                    .val("")
                    .trigger("change");
            }
        },
        show: function () {
            this.base();
            this._highlightOption($(this._popupItem.find("dds-option")[this.selectedNumber]));
        },
        hasValues: function () {
            return !!this._popupItem.html();
        },
        onlyValueJustSelected: function () {
            var children = this._popupItem.children();
            return children.length === 1 && children.text() === this._bindItem.val();
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._url = options.url;
        },
        request: function (value) {
            $.ajax({
                type: "POST",
                url: this._url,
                data: {value: value},
                context: this,
                beforeSend: this.handle("requestBegin"),
                complete: this.handle("requestComplete"),
                error: this.handle("requestError"),
                success: this.handle("requestSuccess")
            });
        }
    }),
    create: function (bindItem) {
        var popupItem = $("<div/>", {
            "class": "dds-options",
            "id": bindItem.attr("id") + "_Options",
            css: {
                width: bindItem.outerWidth() - 2
            }
        });
        var url = bindItem.data("url");
        bindItem.data("autocompleteInitiated", true);
        var hidden = bindItem.next();
        if (hidden.length === 0) {
            hidden = bindItem.parent().next();
        }

        return new DM.Autocomplete({
            popupItem: popupItem,
            bindItem: bindItem,
            hidden: hidden,
            url: url
        });
    }
});