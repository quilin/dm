DM.Dropdown = DM.PopupBase.extend({
    constructor: function (options, view) {
        this.base(options, view || new DM.Dropdown.View(options));

        this.disabled = false;
        this._remainOpenOnSelect = options.remainOpenOnSelect;

        this.__init();
        this.__attachEventListeners();
    },
    __init: function() {
        if (this._view.disabled) {
            this.disable();
        }
    },
    __attachEventListeners: function () {

        this.base();

        this._view.on("input.formReset", function () {
            this.trigger("input.formReset");
        }, this);

        this._view.on("inputClick", function () {
            this.show();
        }, this);
        this._view.on("inpuKey", function (evt) {
            var btn = evt.keyCode || evt.which;
            if (btn === 40 || btn === 32) { // down arrow or spacebar
                this.show();
                evt.preventDefault();
            }
        }, this);
        this._view.on("optionClick", function (option, evt) {
            if (!this._remainOpenOnSelect) {
                this.hide();
            }
            if (!evt.isDefaultPrevented()) {
                this._view.selectOption(option);
            }
            this.trigger("select", option, evt);
        }, this);
        this._view.on("optionKey", function (option, evt) {
            var btn = evt.keyCode || evt.which;
            if (btn === 40) { // down arrow
                this._view.focusNext(option);
                evt.preventDefault();
            }
            if (btn === 38) { // up arrow
                this._view.focusPrevious(option);
                evt.preventDefault();
            }
            if (btn === 27 || btn === 9) { // escape or tab
                if (!this._remainOpenOnSelect) {
                    this.hide();
                }
            }
        }, this);
    },
    show: function() {
        if (!this.disabled) {
            this.base();
        }
    },
    disable: function() {
        if (!this.disabled) {
            this.disabled = true;
            this._view.disable();
        }
        this.hide();
    },
    enable: function() {
        if (this.disabled) {
            this.disabled = false;
            this._view.enable();
        }
    },
    getValue: function() {
        return this._view.getValue();
    },
    setValue: function (value) {
        this._view.setValue(value);
    },
    addOption: function(optionData) {
        this._view.addOption(optionData);
    },
    selectOption: function(index) {
        this._view.selectOptionManually(index);
    },
    getOptions: function() {
        return this._view.getOptions();
    }
}, {
    View: DM.PopupBase.View.extend({
        constructor: function (options) {
            options.bindItem = options.bindItem;
            options.popupItem = options.popupItem;

            this.base(options);

            this._hidden = options.hidden;
            this._input = this._bindItem.children(".dds-select-input");
            this._getOptionLinks().not(".dds-option-disabled").each(function (i) { $(this).data("number", i); });

            var defaultSelectedNumber = parseInt(this._bindItem.attr("data-number"));
            this.selectedNumber = isNaN(defaultSelectedNumber) ? 0 : defaultSelectedNumber;
            this._value = this._input.text();
            this._defaultText = this._input.html();
            this._defaultValue = this._hidden ? this._hidden.val() : null;

            this.disabled = !!this._bindItem.attr("disabled");

            this._bindItem.attr("data-options-popup-id", this._popupItem.attr("id"));

            this.__attachDropdownEventListeners();
            this.__init();
        },
        __attachDropdownEventListeners: function() {
            var _this = this;
            this._bindItem
                .on("click.showSelect", function (evt) {
                    _this.trigger("inputClick");
                })
                .on("keydown.resolveInputKey", function(evt) {
                    _this.trigger("inputKey", evt);
                });
            this._popupItem
                .on("mouseenter", ".dds-option-link:not('.dds-option-disabled')", function() {
                    _this._highlightOption($(this).parent());
                })
                .on("click.selectOption", ".dds-option-link:not('.dds-option-disabled')", function(evt) {
                    var $this = $(this);
                    _this.trigger("optionClick", $this, evt);
                })
                .on("keydown.resolveOptionKey", ".dds-option-link:not('.dds-option-disabled')", function(evt) {
                    _this.trigger("optionKey", $(this), evt);
                });
            if (this._hidden) {
                this._hidden.on("input.formReset", function () {
                    _this._input.html(_this._defaultText);
                    this.value = _this._defaultValue;
                    _this.trigger("input.formReset");
                });
            }
        },
        __init: function () {
            this.base();

            if (this._bindItem.outerWidth() > this._popupItem.outerWidth())
                this._popupItem.width(this._bindItem.width() + 10);

            this._bindItem.data("initiated", true);
        },
        show: function() {
            this.base();
            this._highlightOption($(this._getOptions()[this.selectedNumber]));
        },
        focusPrevious: function (option) {
            var index = parseInt(option.data("number"));
            var optionLinks = this._getOptionLinks();
            if (index > 0)
                this._highlightOption($(optionLinks[index - 1]).parent());
        },
        focusNext: function (option) {
            var index = parseInt(option.data("number"));
            var optionLinks = this._getOptionLinks();
            if (index < optionLinks.length - 1)
                this._highlightOption($(optionLinks[index + 1]).parent());
        },
        _highlightOption: function(option) {
            this._getOptions().removeClass("dds-option-focused");
            option.addClass("dds-option-focused");
            option.find(".dds-option-link").focus();
        },
        selectOption: function (option) {
            this._input.html(option.html());
            this._value = option.text().replace(/^\s+/g, "").replace(/\s+$/g, "");
            this.selectedNumber = option.parent().prevAll().length;

            if (this._hidden !== undefined)
                this._hidden
                    .val(option.data("value"))
                    .trigger("change");
        },
        selectOptionManually: function(index) {
            $(this._getOptionLinks()[index >= 0 ? index : this._getOptions().length + index - 1]).trigger("click.selectOption");
        },
        focus: function () {
            this._bindItem.focus();
        },
        disable: function() {
            this._bindItem.addClass("dds-disabled");
        },
        enable: function () {
            this._bindItem.removeClass("dds-disabled");
        },
        getValue: function() {
            return this._value;
        },
        setValue: function (value) {
            this._input.text(value);
            this._value = value;
        },
        addOption: function (optionData) {
            var options = this._getOptions();
            var order = (optionData.order || options.length);
            order = order > 0 ? order : options.length + order;

            var $option = $("<div/>", {
                "class": "dds-option"
            });
            $("<a/>", {
                "class": "dds-option-link",
                "data-value": optionData.value,
                "href": optionData.href || "javascript:void(0)",
                text: optionData.text
            }).appendTo($option);

            $option.insertAfter(options[order - 1]);
        },
        getOptions: function () {
            var result = [];
            this._getOptions().each(function () {
                var $valueItem = $(this).children(".dds-option-link");
                result.push({
                    disabled: $(this).attr("class").indexOf("dds-option-disabled") >= 0,
                    selected: $(this).attr("class").indexOf("dds-option-focused") >= 0,
                    value: $valueItem.data("value"),
                    text: $valueItem.text(),
                    html: $valueItem.html()
                });
            });
            return result;
        },
        _getOptions: function() {
            return this._popupItem.find(".dds-option");
        },
        _getOptionLinks: function() {
            return this._popupItem.find(".dds-option-link");
        }
    })
});